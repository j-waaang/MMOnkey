using JYW.ThesisMMO.MMOServer.Entities;
using JYW.ThesisMMO.MMOServer.Properties;

namespace JYW.ThesisMMO.MMOServer.CSAIM {

    /// <summary> 
    /// Creates the appropriate filter depending on the config file setting.
    /// </summary>
    internal static class FilterFactory {
        public static PositionFilter GetPositionFilter(ClientEntity entity) {
            if (Settings.Default.UseIndividualUpdateRates) {
                return new IntervalledFilter(entity);
            }
            return new DummyFilter(entity);
        }
    }
}
