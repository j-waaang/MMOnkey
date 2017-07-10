using JYW.ThesisMMO.Common.Types;
using JYW.ThesisMMO.MMOServer.Entities.Attributes;
using JYW.ThesisMMO.MMOServer.Peers;

namespace JYW.ThesisMMO.MMOServer.Entities {
    internal class ClientEntity : Entity {

        private const float InterestRadius = 10f;
        private InterestArea m_InterestArea;

        public ClientEntity(string name, Vector position, Attribute[] attributes, MMOPeer peer) : base(name, position, attributes, peer) {
            m_InterestArea = new InterestArea(this, InterestRadius);
        }
    }
}
