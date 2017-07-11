namespace JYW.ThesisMMO.MMOServer {
    /// <summary>
    /// Item notifies interest areas via regions this item exits and enters.
    /// </summary>
    internal class EntityRegionChangedMessage {

        public EntityRegionChangedMessage(Region from, Region to, Entity entity) {
            From = from;
            To = to;
            Entity = entity;
        }

        public Region From { get; }
        public Region To { get; }
        public Entity Entity { get; }
    };
}
