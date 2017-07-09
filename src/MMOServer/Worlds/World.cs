using System;
using System.Linq;
using System.Collections.Generic;

using Photon.SocketServer;
using ExitGames.Logging;

namespace JYW.ThesisMMO.MMOServer {

    using JYW.ThesisMMO.Common.Codes;
    using JYW.ThesisMMO.Common.Types;
    using JYW.ThesisMMO.MMOServer.Entities.Attributes.Modifiers;
    using JYW.ThesisMMO.MMOServer.Events;
    using JYW.ThesisMMO.MMOServer.Events.ActionEvents;
    using JYW.ThesisMMO.MMOServer.Entities.Attributes;
    using Targets;
    using Worlds;
    using System.Diagnostics;

    /// <summary> 
    /// World split into regions.
    /// </summary>
    internal class World : IDisposable {

        public static World Instance = new World();

        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        private Dictionary<string, Entity> m_Entities = new Dictionary<string, Entity>();

        private Region[,] m_Regions = new Region[m_RegionsWidth, m_RegionsWidth];
        private const float m_RegionSize = 10f;
        private const int m_RegionsWidth = 10;

        private World() {
            CreateRegions();
        }

        private void CreateRegions() {
            var minVal = m_RegionSize * m_RegionsWidth * 0.5f * -1f;
            var minToMax = new Vector(m_RegionSize, m_RegionSize);

            for(var x = 0; x < 10; x++) {
                for (var z = 0; z < 10; z++) {
                    var min = new Vector(minVal + x * m_RegionSize, minVal + z * m_RegionSize);
                    var max = min + minToMax;
                    m_Regions[x, z] = new Region(new BoundingBox2D(min, max));
                }
            }
        }

        /// <summary>
        /// Adding a entity to the game world.
        /// </summary>
        public void AddEntity(Entity newEntity) {
            var sendParameters = new SendParameters { Unreliable = false, ChannelId = 0 };
            var eventData = newEntity.GetNewEntityEventData();

            foreach (Entity entity in m_Entities.Values) {
                entity.SendEvent(eventData, sendParameters);
            }
            m_Entities.Add(newEntity.Name, newEntity);
        }

        public Entity GetEntity(string name) {
            return m_Entities[name];
        }

        /// <summary>
        /// Removes the entity from the list.
        /// </summary>
        public void RemoveEntity(string name) {
            Debug.Assert(m_Entities.ContainsKey(name), "The entity you want to remove does not exist in this region.");

            var ev = new RemovePlayerEvent() {
                Username = name,
            };
            IEventData eventData = new EventData((byte)EventCode.RemovePlayer, ev);
            ReplicateMessage(name, eventData, BroadcastOptions.All);
            m_Entities.Remove(name);
        }

        public void NotifyEntityAboutExistingPlayers(string username) {
            var entityToNotify = m_Entities[username];

            foreach (Entity newPlayer in m_Entities.Values) {
                if (newPlayer == entityToNotify) { continue; }

                //if (Vector.Distance(existingEntity.Position, entityToNotify.Position) > InterestDistance) { continue; }

                var newPlayerEv = new NewPlayerEvent() {
                    Name = newPlayer.Name,
                    Position = newPlayer.Position,
                    CurrentHealth = ((IntAttribute) newPlayer.GetAttribute(AttributeCode.Health)).GetValue(),
                    MaxHealth = ((IntAttribute) newPlayer.GetAttribute(AttributeCode.MaxHealth)).GetValue()
                };



                IEventData eventData = new EventData((byte)EventCode.NewPlayer, newPlayerEv);
                var sendParameters = new SendParameters { Unreliable = false, ChannelId = 0 };
                entityToNotify.SendEvent(eventData, sendParameters);
            }
        }

        public void MoveEntity(string username, Vector position) {

            var movedEntity = m_Entities[username];
            movedEntity.Position = position;
            var moveEvent = new MoveEvent() {
                Name = movedEntity.Name,
                Position = movedEntity.Position
            };

            IEventData eventData = new EventData((byte)EventCode.Move, moveEvent);
            var sendParameters = new SendParameters { Unreliable = true, ChannelId = 0 };

            foreach (Entity entity in m_Entities.Values) {
                if(entity == movedEntity) { continue; }
                entity.SendEvent(eventData, sendParameters);
            }

            //var movedEntity = m_Entities[username];
            //IEventData eventData;
            //MoveEvent moveEvent;
            //var sendParameters = new SendParameters { Unreliable = true, ChannelId = 0 };

            //var interestedEntities = GetEntitiesInInterestRange(movedEntity);
            //foreach (Entity interestedEntity in interestedEntities) {
            //    if (Vector.Distance(position, interestedEntity.Position) < InterestDistance) {
            //        moveEvent = new MoveEvent() {
            //            Username = interestedEntity.Name,
            //            Position = interestedEntity.Position
            //        };
            //        eventData = new EventData((byte)EventCode.Move, moveEvent);

            //        movedEntity.SendEvent(eventData, sendParameters);
            //    }
            //}

            //movedEntity.Position = position;
            //// TODO: check if new position is valid

            //moveEvent = new MoveEvent() {
            //    Username = movedEntity.Name,
            //    Position = movedEntity.Position
            //};
            //eventData = new EventData((byte)EventCode.Move, moveEvent);

            //foreach (Entity entity in interestedEntities) {
            //    entity.SendEvent(eventData, sendParameters);
            //}
        }

        //private List<Entity> GetEntitiesInInterestRange(Entity centerEntity) {
        //    var entities = new List<Entity>();
        //    foreach (Entity entity in m_Entities.Values) {
        //        if (entity == centerEntity) { continue; }
        //        if (Vector.Distance(entity.Position, centerEntity.Position) > InterestDistance) { continue; }

        //        entities.Add(entity);
        //    }

        //    return entities;
        //}

        public bool CanPerformAction(string actionSource) {
            Entity entity = null;
            m_Entities.TryGetValue(actionSource, out entity);
            if (entity == null) {
                log.ErrorFormat("{0} entity requesting action does not exist.", actionSource);
                return false;
            }
            return entity.IsIdle();
        }

        public bool CanPerformAction(string actionSource, ActionCode action) {
            Entity entity = null;
            m_Entities.TryGetValue(actionSource, out entity);
            if(entity == null) {
                log.ErrorFormat("{0} entity requesting action does not exist.", actionSource);
                return false;
            }
            return entity.CanPerformAction(action);
        }

        public bool CanPerformAction(string actionSource, ActionCode action, Target target) {
            if(!CanPerformAction(actionSource, action)) { return false; }

            // TODO: Test distance
            return true;
        }

        /// <summary>
        /// Use this to replicate a attribute change. Do not use for position changes.
        /// </summary>
        public void ReplicateMessage(string src, IEventData eventData, BroadcastOptions options ) {
            var sendParameters = new SendParameters { Unreliable = false, ChannelId = 0 };

            foreach (Entity entity in m_Entities.Values) {
                if(options == BroadcastOptions.AllExceptMsgOwner && entity.Name == src) { continue; }

                entity.SendEvent(eventData, sendParameters);
            }
        }

        public IEnumerable<string> GetEntitesInArea(AreaTarget area) {
            var inArea = m_Entities.Where(a => area.IsEntityInArea(a.Value));
            return inArea.Select(a => a.Value.Name);
        }

        public void ApplyModifier(Target target, Modifier modifier) {
            switch (target.TargetType) {
                case TargetType.Entity:
                    ApplyModifier(((EntityTarget)target).TargetName, modifier);
                    break;
                case TargetType.Point:
                    throw new NotImplementedException();
                case TargetType.Area:
                    var areaTarget = (AreaTarget)target;

                    //Create array because elements in dictionary could get removed in the process.
                    var entities = m_Entities.Values.ToArray();
                    foreach(Entity entity in entities) {
                        if (areaTarget.IsEntityInArea(entity)) {
                            modifier.ApplyEffect(entity);
                            log.InfoFormat("Applying aoe to {0}.", entity.Name);
                        }
                    }
                    break;
            }
        }

        public void ApplyModifier(string target, Modifier modifier) {
            Entity entity = null;
            m_Entities.TryGetValue(target, out entity);
            if(entity == null) {
                log.InfoFormat("Trying to apply modifier on {0} but {0} doesn't exist.", target);
                return;
            }
            modifier.ApplyEffect(entity);
        }

        public void Dispose() {
            Instance = null;
        }

        private Region GetRegionFromPoint(Vector point) {
            var minVal = m_RegionSize * m_RegionsWidth * 0.5f * -1f;

            Debug.Assert(minVal < point.X, "Cannot evaluate a point outside of the game world");
            Debug.Assert(minVal < point.Z, "Cannot evaluate a point outside of the game world");
            Debug.Assert(-minVal > point.X, "Cannot evaluate a point outside of the game world");
            Debug.Assert(-minVal > point.Z, "Cannot evaluate a point outside of the game world");

            var x = (int)Math.Floor((point.X - minVal) % m_RegionSize);
            var z = (int)Math.Floor((point.Z - minVal) % m_RegionSize);

            Debug.Assert(x < m_RegionsWidth, "Error in calculation. Region does not exist.");
            Debug.Assert(z < m_RegionsWidth, "Error in calculation. Region does not exist.");

            return m_Regions[x,z];
        }
    }
}
