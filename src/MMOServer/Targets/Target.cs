using ExitGames.Logging;

namespace JYW.ThesisMMO.MMOServer.Targets {

    abstract internal class Target {

        protected static readonly ILogger log = LogManager.GetCurrentClassLogger();

        public TargetType TargetType { get; protected set; }
    }
}
