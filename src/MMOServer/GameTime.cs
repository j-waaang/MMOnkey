using ExitGames.Logging;
using System;
using System.Diagnostics;

namespace JYW.ThesisMMO.MMOServer {
    internal static class GameTime {

        /// <summary> 
        /// Time in MS since GameTime initialization.
        /// </summary>
        public static long TimeMs {
            get {
                return stopwatch.ElapsedMilliseconds;
            }
        }

        /// <summary> 
        /// TimeSpan since GameTime initialization.
        /// </summary>
        public static TimeSpan Time {
            get {
                return stopwatch.Elapsed;
            }
        }

        private static Stopwatch stopwatch = new Stopwatch();
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        static GameTime() {
            stopwatch.Start();
            log.InfoFormat("Initialized GameTime.");
        }
    }
}
