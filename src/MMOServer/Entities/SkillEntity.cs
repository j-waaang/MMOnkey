using Photon.SocketServer;

namespace JYW.ThesisMMO.MMOServer.Entities {

    using Events.EntityEvents;
    using JYW.ThesisMMO.Common.Codes;
    using JYW.ThesisMMO.Common.Types;

    internal class SkillEntity : Entity {

        public ActionCode ActionCode { get; private set; }
        public string Caster { get; private set; }

        public SkillEntity(string caster, string name, Vector position, ActionCode actionCode) : base(name, position, null, null) {
            log.InfoFormat("Creating skillentity with actioncode {0}", ActionCode);

            ActionCode = actionCode;
            Caster = caster;
        }

        public override IEventData GetNewEntityEventData() {
            log.InfoFormat("Creating skillentity event data with actioncode {0}", ActionCode.ToString());
            var newPlayerEv = new NewSkillEntityEvent() {
                Name = Name,
                Position = Position,
                ActionCode = (int)ActionCode,
                EntityType = (int)EntityType.Skill
            };

            return new EventData((byte)EventCode.NewEntity, newPlayerEv);
        }
    }
}
