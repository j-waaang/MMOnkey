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

        internal Entity CreatePeerControlledEntity(MMOPeer peer, EnterWorldRequest operation) {
            var position = GetRandomWorldPosition();
            var maxHealth = GetMaxHealth((WeaponCode)operation.Weapon);
            var attributes = new Attribute[4];
            attributes[0] = new IntAttribute(maxHealth, AttributeCode.MaxHealth);
            attributes[1] = new IntHealthAttribute(maxHealth);
            attributes[2] = new ActionStateAttribute();
            attributes[3] = new FloatAttribute(0.2f, AttributeCode.Speed);

            return new Entity(operation.Name, position, attributes, peer);
        }

        internal void CreateSkillEntity(string id, ActionCode actionCode, Vector startPosition) {

            var name = actionCode.ToString() + id;

            var stringType = aiEntityNameSpace + actionCode.ToString() + "AI";
            var actionType = Type.GetType(stringType);

            if (actionType == null) {
                log.ErrorFormat("Type {0} was not found.", stringType);
                return;
            }

            Entity skillEntity = new SkillEntity(name, startPosition, actionCode);
            Activator.CreateInstance(actionType, skillEntity);
        }

        internal void CreateAIBot(string name, Vector startPosition) {
            var position = startPosition;
            var maxHealth = GetMaxHealth(WeaponCode.Axe);
            var attributes = new Attribute[4];
            attributes[0] = new IntAttribute(maxHealth, AttributeCode.MaxHealth);
            attributes[1] = new IntHealthAttribute(maxHealth);
            attributes[2] = new ActionStateAttribute();
            attributes[3] = new FloatAttribute(0.2f, AttributeCode.Speed);

            new TestBot(new Entity(name, position, attributes, null));
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
