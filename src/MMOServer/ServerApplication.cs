namespace JYW.ThesisMMO.MMOServer {
    using Common.Types;
    using Photon.SocketServer;
    using Peers;
    using Protocol = Common.Types.Protocol;
    using ExitGames.Logging;
    using ExitGames.Logging.Log4Net;
    using log4net.Config;
    using System.IO;
    using AI;

    sealed class ServerApplication : ApplicationBase {

        private static readonly ILogger log = LogManager.GetCurrentClassLogger();
        private World m_World;
        private AIModule m_AIModule;

        protected override PeerBase CreatePeer(InitRequest initRequest) {
            return new MMOPeer(initRequest);
        }
        protected override void Setup() {
            SetupLogger();
            RegisterTypes();
            CreateWorld();
            StartAIModule();
        }
        private void SetupLogger() {
            log4net.GlobalContext.Properties["Photon:ApplicationLogPath"] = Path.Combine(this.ApplicationRootPath, "log");
            var configFileInfo = new FileInfo(Path.Combine(this.BinaryPath, "log4net.config"));
            if (configFileInfo.Exists) {
                LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);
                XmlConfigurator.ConfigureAndWatch(configFileInfo);
            }

            log.DebugFormat("------------------------Started------------------------");
        }
        private static void RegisterTypes() {
            Photon.SocketServer.Protocol.TryRegisterCustomType(
                typeof(Vector),
                (byte)Protocol.CustomTypeCodes.Vector,
                Protocol.SerializeVector,
                Protocol.DeserializeVector);
        }
        private void CreateWorld() {
            m_World = new World();
            log.DebugFormat("Created game world.");
        }
        protected override void TearDown() {
            log.DebugFormat("Tear Down");
        }

        private void StartAIModule() {
            m_AIModule = new AIModule();
        }

        private void CreateTestBots() { }
    }
}
