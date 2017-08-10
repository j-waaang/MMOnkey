using Photon.SocketServer;

namespace JYW.ThesisMMO.MMOServer.Entities {

    using JYW.ThesisMMO.Common.Codes;
    using JYW.ThesisMMO.Common.Types;
    using JYW.ThesisMMO.MMOServer.Events;
    using JYW.ThesisMMO.MMOServer.Peers;
    using JYW.ThesisMMO.MMOServer.Entities.Attributes;
    using Skills;

    internal class ClientEntity : Entity {
        public SkillCollection EquippedSkills { get; }
        private static readonly SendParameters DefaultSendParameters = new SendParameters { Unreliable = false, ChannelId = 0 };

        public ClientEntity(string name, Vector position, Attribute[] attributes, MMOPeer peer, int[] skillData) : base(name, position, attributes, peer) {
            EquippedSkills = new SkillCollection(skillData);
        }

        protected override void SetInterestArea() {
            m_InterestArea = new ClientInterestArea(this);
        }

        /// <summary> 
        /// Use this method to update the peer.
        /// </summary>
        public override SendResult SendEvent(IEventData eventData, SendParameters sendParameters) {
            return Peer.SendEvent(eventData, sendParameters);
        }

        /// <summary> 
        /// Use this method to update the peer.
        /// Default sendparameters are used. Reliable. Channel 0
        /// </summary>
        public override SendResult SendEvent(IEventData eventData) {
            return SendEvent(eventData, DefaultSendParameters);
        }

        public override NewEntityEvent GetEntitySnapshot() {
            return new NewPlayerEvent() {
                Name = Name,
                Position = Position,
                CurrentHealth = ((IntAttribute)GetAttribute(AttributeCode.Health)).GetValue(),
                MaxHealth = ((IntAttribute)GetAttribute(AttributeCode.MaxHealth)).GetValue()
            };
        }

        public override bool CanPerformAction(ActionCode action) {
            if(!base.CanPerformAction(action)) { return false; }
            var x = EquippedSkills.CanActivateSkill(action);
            log.InfoFormat("{0} can perfomr action test is {1}", action, x);

            return x;
        }

        public override void Dispose() {
            base.Dispose();
        }

        public override void Die() {
            PublishDeath();
            base.Die();
        }

        private void PublishDeath() {
            var dataContract = new EntityEvent() { Username = Name };
            IEventData evData = new EventData((byte)EventCode.EntityDeath, dataContract);
            var msg = new EventMessage(evData, DefaultSendParameters, Events.ActionEvents.BroadcastOptions.All, Name);
            PublishEvent(msg);
        }
    }
}
