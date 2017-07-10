namespace JYW.ThesisMMO.MMOServer {
    /// <summary>
    /// Item notifies interest areas via regions this item exits and enters.
    /// </summary>
    internal class EntityRegionChangedMessage {
        public EntityRegionChangedMessage(Region r0, Region r1, Entity entity) {
            this.Region0 = r0;
            this.Region1 = r1;
            this.Entity = entity;
        }
        public Region Region0 { get; private set; }
        public Region Region1 { get; private set; }
        public Entity Entity { get; private set; }
    };
}
