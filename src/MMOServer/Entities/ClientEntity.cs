using ExitGames.Concurrency.Fibers;
using Photon.SocketServer;

namespace JYW.ThesisMMO.MMOServer.Entities {

    using JYW.ThesisMMO.Common.Codes;
    using JYW.ThesisMMO.Common.Types;
    using JYW.ThesisMMO.MMOServer.Events;
    using JYW.ThesisMMO.MMOServer.Peers;
    using JYW.ThesisMMO.MMOServer.Entities.Attributes;

    internal class ClientEntity : Entity {

        private static readonly SendParameters DefaultSendParameters = new SendParameters { Unreliable = false, ChannelId = 0 };

        public ClientEntity(string name, Vector position, Attribute[] attributes, MMOPeer peer) : base(name, position, attributes, peer) {
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

        public override IEventData GetEntitySnapshot() {
            var newPlayerEv = new NewPlayerEvent() {
                Name = Name,
                Position = Position,
                CurrentHealth = ((IntAttribute)GetAttribute(AttributeCode.Health)).GetValue(),
                MaxHealth = ((IntAttribute)GetAttribute(AttributeCode.MaxHealth)).GetValue()
            };
            return new EventData((byte)EventCode.NewPlayer, newPlayerEv);
        }
    }
}
