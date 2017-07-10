using System;
using ExitGames.Logging;

namespace JYW.ThesisMMO.MMOServer {

    using JYW.ThesisMMO.Common.Codes;
    using JYW.ThesisMMO.Common.Types;
    using JYW.ThesisMMO.MMOServer.Requests;
    using JYW.ThesisMMO.MMOServer.Peers;
    using JYW.ThesisMMO.MMOServer.Entities.Attributes;
    using AI;
    using Entities;
    using ActionObjects;

    internal sealed class EntityFactory {

        private static EntityFactory m_Instance = null;
        private static readonly object m_Lock = new object();
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        private const string aiEntityNameSpace = "JYW.ThesisMMO.MMOServer.AI.";

        private EntityFactory() {
        }

        public static EntityFactory Instance {
            get {
                if (m_Instance == null) {
                    lock (m_Lock) {
                        if (m_Instance == null) {
                            m_Instance = new EntityFactory();
                        }
                    }
                }
                return m_Instance;
            }
        }

        internal Entity CreateClientEntity(MMOPeer peer, EnterWorldRequest operation) {
            var position = GetRandomWorldPosition();
            var maxHealth = GetMaxHealth((WeaponCode)operation.Weapon);
            var attributes = new Attribute[4];
            attributes[0] = new IntAttribute(maxHealth, AttributeCode.MaxHealth);
            attributes[1] = new IntHealthAttribute(maxHealth);
            attributes[2] = new ActionStateAttribute();
            attributes[3] = new FloatAttribute(7f, AttributeCode.Speed);

            var entity = new Entity(operation.Name, position, attributes, peer);
            World.Instance.AddEntity(entity);
            return entity;
        }

        internal void CreateSkillEntity(ActionObject actionObject, Vector startPosition) {
            var entityName = ((ActionCode)actionObject.Code).ToString() + actionObject.GetNextID();

            var stringType = aiEntityNameSpace + (ActionCode)actionObject.Code + "AI";
            var actionType = Type.GetType(stringType);

            if (actionType == null) {
                log.ErrorFormat("Type {0} was not found.", stringType);
                return;
            }

            var skillEntity = new SkillEntity(actionObject.ActionSource, entityName, startPosition, (ActionCode)actionObject.Code);
            World.Instance.AddEntity(skillEntity);
            Activator.CreateInstance(actionType, skillEntity);
        }

        internal void CreateSkillEntity(string caster, string id, ActionCode actionCode, Vector startPosition) {
            log.InfoFormat("Factory received skill entity creation request with code {0}", actionCode);
            var name = actionCode.ToString() + id;

            var stringType = aiEntityNameSpace + actionCode.ToString() + "AI";
            var actionType = Type.GetType(stringType);

            if (actionType == null) {
                log.ErrorFormat("Type {0} was not found.", stringType);
                return;
            }

            var skillEntity = new SkillEntity(caster, name, startPosition, actionCode);
            World.Instance.AddEntity(skillEntity);
            Activator.CreateInstance(actionType, skillEntity);
        }

        internal void CreateAIBot(string name, Vector startPosition, bool canMove) {
            var position = startPosition;
            var maxHealth = GetMaxHealth(WeaponCode.Axe);
            var attributes = new Attribute[4];
            attributes[0] = new IntAttribute(maxHealth, AttributeCode.MaxHealth);
            attributes[1] = new IntHealthAttribute(maxHealth);
            attributes[2] = new ActionStateAttribute();
            attributes[3] = new FloatAttribute(0.2f, AttributeCode.Speed);

            var entity = new Entity(name, position, attributes, null);
            World.Instance.AddEntity(entity);
            var aiEntity = new TestBot(entity);
            aiEntity.canMove = canMove;
        }
        
        // TODO: Change design so health does not depend on weapon.
        private static int GetMaxHealth(WeaponCode weapon) {
            switch (weapon) {
                case WeaponCode.Axe:
                    return 100;
                case WeaponCode.Bow:
                    return 70;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        // TODO: Actually return a random position.
        private static Vector GetRandomWorldPosition() {
            return Vector.Zero;
        }
    }
}
