using ExitGames.Logging;
using System.Collections.Generic;
using System;

namespace JYW.ThesisMMO.MMOServer {
    using AI;
    using JYW.ThesisMMO.Common.Types;

    internal static class EntityScenario {
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();
        private static readonly string[] Teams = new string[] { "Mob", "Blue" };
        private static int m_LastTeam = 0;
        private static int m_LastBot = 0;

        private static List<AIEntity> Bots = new List<AIEntity>();

        public static void CreateHomogeneousScenario() {
            var start = World.Instance.WorldBounds.Min + Vector.One2D * World.RegionSize * 0.5f;
            var end = World.Instance.WorldBounds.Max - Vector.One2D * World.RegionSize * 0.5f;

            for (var X = start.X; X <= end.X; X += 10) {
                for (var Z = start.Z; Z <= end.Z; Z += 10) {
                    var team = Teams[GetNextTeamIndex()];
                    var name = string.Format("{0}-{1} {2} Entity", X, Z, team);
                    if (m_LastBot == 0) {
                        Bots.Add(EntityFactory.Instance.CreateAIBot2(name, new Vector(X, Z), team, true));
                    }
                    else {
                        Bots.Add(EntityFactory.Instance.CreateAIBot(name, new Vector(X, Z), team, true));
                    }
                    m_LastBot++;
                    m_LastBot %= 2;
                }
            }
        }

        public static void CreateCentralizedScenario() {
            var start = new Vector(-9F, -9F);
            var end = new Vector(19F, 19F);

            //var team = Teams[GetNextTeamIndex()];
            ////var name = string.Format("{0}-{1} Entity", X, Z);
            //Bots.Add(EntityFactory.Instance.CreateAIBot("1-1", new Vector(1F, 1F), team, true));
            //Bots.Add(EntityFactory.Instance.CreateAIBot("2-2", new Vector(2F, 2F), team, true));
            //Bots.Add(EntityFactory.Instance.CreateAIBot("3-3", new Vector(3F, 3F), team, true));
            //Bots.Add(EntityFactory.Instance.CreateAIBot("4-4", new Vector(4F, 4F), team, true));


            for (var X = start.X; X <= end.X; X += 3) {
                for (var Z = start.Z; Z <= end.Z; Z += 3) {
                    var team = Teams[GetNextTeamIndex()];
                    var name = string.Format("{0}-{1} Entity", X, Z);
                    if (m_LastBot == 0) {
                        Bots.Add(EntityFactory.Instance.CreateAIBot2(name, new Vector(X, Z), team, true));
                    }
                    else {
                        Bots.Add(EntityFactory.Instance.CreateAIBot(name, new Vector(X, Z), team, true));
                    }
                    m_LastBot++;
                    m_LastBot %= 2;
                }
            }
        }

        private static int GetNextTeamIndex() {
            ++m_LastTeam;
            return m_LastTeam % Teams.Length;
        }

        internal static void CreateSingleEnemy() {
            Bots.Add(EntityFactory.Instance.CreateAIBot("John", new Vector(5F, 5F), "Mob", true));
        }
    }
}
