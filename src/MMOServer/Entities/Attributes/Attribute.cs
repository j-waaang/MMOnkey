using ExitGames.Logging;
using Photon.SocketServer;

namespace JYW.ThesisMMO.MMOServer.Entities.Attributes {
    using Events.ActionEvents;
    using JYW.ThesisMMO.Common.Codes;

    internal abstract class Attribute {

        protected static readonly ILogger log = LogManager.GetCurrentClassLogger();

        protected Entity m_Entity;

        internal protected AttributeCode AttributeCode { get; protected set; }

        internal Attribute(AttributeCode attributeCode) {
            AttributeCode = attributeCode;
        }

        internal void SetEntity(Entity entity) {
            m_Entity = entity;
        }

        protected void PublishChange(IEventData eventData, BroadcastOptions options) {
            var sendParameters = new SendParameters { Unreliable = false, ChannelId = 0 };
            var msg = new EventMessage(eventData, sendParameters, options, m_Entity.Name);
            m_Entity.PublishEvent(msg);
        }
    }
}
