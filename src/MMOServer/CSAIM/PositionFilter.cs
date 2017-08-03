using JYW.ThesisMMO.Common.Codes;
using JYW.ThesisMMO.MMOServer.Entities;
using JYW.ThesisMMO.MMOServer.Events;
using Photon.SocketServer;

namespace JYW.ThesisMMO.MMOServer.CSAIM {
    abstract class PositionFilter {

        protected static readonly SendParameters PositionSendParameters = new SendParameters() { ChannelId = (byte)ChannelId.Position, Unreliable = true };
        protected readonly ClientEntity m_AttachedEntity;

        public PositionFilter(ClientEntity entity) {
            m_AttachedEntity = entity;
        }

        public abstract void OnPositionUpdate(Entity c);

        protected virtual void UpdateClientPosition(Entity entity) {
            EventMessage.CounterEventReceive.Increment();
            var moveEvent = new MoveEvent(entity.Name, entity.Position);
            IEventData eventData = new EventData((byte)EventCode.Move, moveEvent);
            m_AttachedEntity.SendEvent(eventData, PositionSendParameters);
        }
    }
}
