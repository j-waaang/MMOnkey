namespace JYW.ThesisMMO.MMOServer.CSAIM.EnterExitEvents {

    using JYW.ThesisMMO.MMOServer.Properties;

    internal static class EnterExitFilterFactory {

        public static EnterExitFilter CreateEnterExitFilter(Entity entity) {
            if (Settings.Default.FilterKnownSnapshots) {
                return new SnapshotFilter(entity);
            }

            return new DummyEnterExitFilter(entity);
        }
    }
}
