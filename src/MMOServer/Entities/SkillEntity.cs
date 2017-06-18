using Photon.SocketServer;

namespace JYW.ThesisMMO.MMOServer.Entities {

    using Events.EntityEvents;
    using JYW.ThesisMMO.Common.Codes;
    using JYW.ThesisMMO.Common.Types;

    internal class SkillEntity : Entity {

        public ActionCode ActionCode { get; private set; }

        public SkillEntity(string name, Vector position, ActionCode actionCode) : base(name, position, null, null) {
            ActionCode = actionCode;
        }

        public override IEventData GetNewEntityEventData() {
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
