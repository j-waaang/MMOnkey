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
    using System.Linq;
    using System.Diagnostics;

    internal sealed class EntityFactory {

        public static EntityFactory Instance = new EntityFactory();

        private static readonly ILogger log = LogManager.GetCurrentClassLogger();
        private const string aiEntityNameSpace = "JYW.ThesisMMO.MMOServer.AI.";

        private EntityFactory() {}

        internal Entity CreateClientEntity(MMOPeer peer, EnterWorldRequest operation) {
            var position = GetRandomWorldPosition();
            var maxHealth = GetMaxHealth((WeaponCode)operation.Weapon);
            var attributes = new Attribute[] {
                new IntAttribute(maxHealth, AttributeCode.MaxHealth),
                new HealthAttribute(maxHealth),
                new ActionStateAttribute(),
                new FloatAttribute(7f, AttributeCode.Speed)
            };

            var skills = operation.Skills;
            Array.Resize(ref skills, skills.Length + 1);
            skills[skills.Length-1] = operation.Weapon;
            var entity = new ClientEntity(operation.Name, position, operation.Team, attributes, peer, skills);
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

            var skillEntity = new SkillEntity(actionObject.ActionSource, entityName, startPosition, TeamByEntity(actionObject.ActionSource), (ActionCode)actionObject.Code);
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

            var skillEntity = new SkillEntity(caster, name, startPosition, TeamByEntity(caster), actionCode);
            World.Instance.AddEntity(skillEntity);
            Activator.CreateInstance(actionType, skillEntity);
        }

        private string TeamByEntity(string entity) {
            return World.Instance.GetEntity(entity).Team;
        }

        internal TestBot CreateAIBot(string name, Vector startPosition, string team, bool canMove) {
            var position = startPosition;
            var maxHealth = GetMaxHealth(WeaponCode.Axe);
            var attributes = new Attribute[] {
                new IntAttribute(maxHealth, AttributeCode.MaxHealth),
                new HealthAttribute(maxHealth),
                new ActionStateAttribute(),
                new FloatAttribute(0.2f, AttributeCode.Speed)};

            var entity = new Entity(name, position, team, attributes, null);
            var aiEntity = new TestBot(entity);
            aiEntity.canMove = canMove;
            World.Instance.AddEntity(entity);
            return aiEntity;
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
