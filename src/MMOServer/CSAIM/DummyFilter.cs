using JYW.ThesisMMO.MMOServer.Entities;

namespace JYW.ThesisMMO.MMOServer.CSAIM {
    internal class DummyFilter : PositionFilter {

        public DummyFilter(ClientEntity entity)
            : base(entity) { }

        public override void OnPositionUpdate(Entity entity) {
            UpdateClientPosition(entity);
        }
    }
}
