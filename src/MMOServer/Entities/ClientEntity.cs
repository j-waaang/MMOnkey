using JYW.ThesisMMO.Common.Types;
using JYW.ThesisMMO.MMOServer.Entities.Attributes;
using JYW.ThesisMMO.MMOServer.Peers;
using Photon.SocketServer;

namespace JYW.ThesisMMO.MMOServer.Entities {
    internal class ClientEntity : Entity {

        public Region CurrentWorldRegion { get; private set; }

        private const float InterestRadius = 10f;
        private InterestArea m_InterestArea;
        private static readonly SendParameters DefaultSendParameters = new SendParameters { Unreliable = false, ChannelId = 0 };

        public ClientEntity(string name, Vector position, Attribute[] attributes, MMOPeer peer) : base(name, position, attributes, peer) {
            m_InterestArea = new InterestArea(this, InterestRadius);
        }

        public override void Move(Vector position) {
            base.Move(position);
            UpdateInterestManagment();
        }

        public override void EnterRegion() {
            UpdateInterestManagment();
        }

        private void UpdateInterestManagment() {
            var newRegion = World.Instance.GetRegionFromPoint(Position);
            if(CurrentWorldRegion != newRegion) {
                ChangeRegion(CurrentWorldRegion, newRegion);
            }

            m_InterestArea.UpdateInterestManagment();
        }

        private void ChangeRegion(Region from, Region to) {
            var msg = new EntityRegionChangedMessage(from, to, this);

            if (from != null) {
                from.EntityRegionChangedChannel.Publish(msg);
            }
            if (to != null) {
                to.EntityRegionChangedChannel.Publish(msg);
            }
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
    }
}
