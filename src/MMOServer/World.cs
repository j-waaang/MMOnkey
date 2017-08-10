using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

using Photon.SocketServer;
using ExitGames.Logging;

namespace JYW.ThesisMMO.MMOServer {
    using Entities;
    using JYW.ThesisMMO.Common.Codes;
    using JYW.ThesisMMO.Common.Types;
    using JYW.ThesisMMO.MMOServer.Entities.Attributes.Modifiers;
    using JYW.ThesisMMO.MMOServer.Events;
    using JYW.ThesisMMO.MMOServer.Events.ActionEvents;
    using Targets;

    /// <summary> 
    /// World split into regions.
    /// </summary>
    internal class World : IDisposable {

        public static World Instance = new World();

        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        private const float RegionSize = 10f;
        private const int TileDimension = 10;

        private readonly Region[,] m_Regions = new Region[TileDimension, TileDimension];
        private readonly Dictionary<string, Entity> m_Entities = new Dictionary<string, Entity>();
        private readonly BoundingBox2D m_WorldArea;

        private World() {
            var x = RegionSize * TileDimension * 0.5f;
            m_WorldArea = new BoundingBox2D(new Vector(-x, -x), new Vector(x, x));
            CreateRegions();
        }

        private void CreateRegions() {
            var minToMax = new Vector(RegionSize, RegionSize);
            for (var x = 0; x < 10; x++) {
                for (var z = 0; z < 10; z++) {
                    var min = m_WorldArea.Min + new Vector(x * RegionSize, z * RegionSize);
                    var max = min + minToMax;
                    m_Regions[x, z] = new Region(new BoundingBox2D(min, max), x, z);
                }
            }
        }

        /// <summary>
        /// Adding a entity to the game world.
        /// </summary>
        public void AddEntity(Entity newEntity) {
            m_Entities.Add(newEntity.Name, newEntity);
            newEntity.OnAddedToWorld();
        }

        public Entity GetEntity(string name) {
            return m_Entities[name];
        }

        /// <summary>
        /// Removes the entity from the list.
        /// </summary>
        public void RemoveEntity(string name) {
            Debug.Assert(m_Entities.ContainsKey(name), "The entity you want to remove does not exist.");
            m_Entities[name].Dispose();
            m_Entities.Remove(name);
        }

        public void DisconnectPeer(string name) {
            if (!m_Entities.ContainsKey(name)) { return; }
            RemoveEntity(name);
        }

        public void MoveEntity(string username, Vector position) {
            m_Entities[username].Move(position);
        }

        public bool CanPerformAction(string actionSource, ActionCode action) {
            Entity entity = null;
            m_Entities.TryGetValue(actionSource, out entity);
            if (entity == null) {
                log.ErrorFormat("{0} entity requesting action does not exist.", actionSource);
                return false;
            }
            return entity.CanPerformAction(action);
        }

        public bool CanPerformAction(string actionSource, ActionCode action, string target, float maxDistance) {
            if (!CanPerformAction(actionSource, action)) { return false; }
            if (Vector.Distance(m_Entities[actionSource].Position, m_Entities[target].Position) > maxDistance) { return false; }
            return true;
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
                    foreach (Entity entity in entities) {
                        if (areaTarget.IsEntityInArea(entity)) {
                            modifier.ApplyEffect(entity);
                            log.InfoFormat("Applying aoe to {0}.", entity.Name);
                        }
                    }
                    break;
            }
        }

        public void SetSkillCooldown(string entity, ActionCode skill) {
            Debug.Assert(m_Entities.ContainsKey(entity));
            Debug.Assert(m_Entities[entity].GetType() == typeof(ClientEntity));

            var player = m_Entities[entity] as ClientEntity;
            player.EquippedSkills.SetSkillOnCooldown(skill);
        }

        public void ApplyModifier(string target, Modifier modifier) {
            Entity entity = null;
            m_Entities.TryGetValue(target, out entity);
            if (entity == null) {
                log.InfoFormat("Trying to apply modifier on {0} but {0} doesn't exist.", target);
                return;
            }
            modifier.ApplyEffect(entity);
        }

        public void Dispose() {
            foreach (var region in m_Regions) {
                region.Dispose();
            }
            foreach (var entity in m_Entities.Values) {
                entity.Dispose();
            }
            Instance = null;
        }

        public Region GetRegionFromPoint(Vector point) {
            var minVal = RegionSize * TileDimension * 0.5f * -1f;
            var x = (int)Math.Floor(Math.Abs(point.X - minVal) / RegionSize);
            var z = (int)Math.Floor(Math.Abs(point.Z - minVal) / RegionSize);

            Debug.Assert(m_Regions[x, z].Boundaries.Contains(point));
            return m_Regions[x, z];
        }

        public IEnumerable<Region> Get9RegionsFromPoint(Vector point) {
            var minVal = RegionSize * TileDimension * 0.5f * -1f;
            var x = (int)Math.Floor(Math.Abs(point.X - minVal) / RegionSize);
            var z = (int)Math.Floor(Math.Abs(point.Z - minVal) / RegionSize);

            for (int xi = x - 1; xi <= x + 1; xi++) {
                if (xi < 0 || xi >= 10) { continue; }

                for (int zi = z - 1; zi <= z + 1; zi++) {
                    if (zi < 0 || zi >= 10) { continue; }

                    yield return m_Regions[xi, zi];
                }
            }
            yield break;
        }
    }
}
