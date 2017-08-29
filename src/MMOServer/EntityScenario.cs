using ExitGames.Logging;
using System.Collections.Generic;

namespace JYW.ThesisMMO.MMOServer {
    using AI;
    using JYW.ThesisMMO.Common.Types;

    internal static class EntityScenario {
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();
        private static readonly string[] Teams = new string[] { "Mob", "Blue" };
        private static int m_LastTeam = 0;

        private static List<TestBot> Bots = new List<TestBot>();

        public static void CreateHomogeneousScenario() {
            var start = World.Instance.WorldBounds.Min + Vector.One2D * World.RegionSize * 0.5f;
            var end = World.Instance.WorldBounds.Max - Vector.One2D * World.RegionSize * 0.5f;

            for (var X = start.X; X <= end.X; X += 6) {
                for (var Z = start.Z; Z <= end.Z; Z += 6) {
                    var name = string.Format("{0}-{1} Entity", X, Z);
                    Bots.Add(EntityFactory.Instance.CreateAIBot(name, new Vector(X, Z), Teams[GetNextTeamIndex()], true));
                }
            }
        }

        public static void CreateCentralizedScenario() {
            var start = World.Instance.WorldBounds.Min + Vector.One2D * World.RegionSize * 0.5f;
            var end = World.Instance.WorldBounds.Max - Vector.One2D * World.RegionSize * 0.5f;

            for (var X = start.X; X <= end.X; X += 15) {
                for (var Z = start.Z; Z <= end.Z; Z += 15) {
                    var name = string.Format("{0}-{1} Entity", X, Z);
                    Bots.Add(EntityFactory.Instance.CreateAIBot(name, new Vector(X, Z), Teams[GetNextTeamIndex()], false));
                }
            }
        }

        private static int GetNextTeamIndex() {
            ++m_LastTeam;
            return m_LastTeam % Teams.Length;
        }
    }
}
