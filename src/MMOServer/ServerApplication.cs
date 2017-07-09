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
    using System.Diagnostics;

    /// <summary> 
    /// Main class of the server.
    /// </summary>
    sealed class ServerApplication : ApplicationBase {

        private static readonly ILogger log = LogManager.GetCurrentClassLogger();
        private World m_World;

        protected override PeerBase CreatePeer(InitRequest initRequest) {
            return new MMOPeer(initRequest);
        }

        protected override void Setup() {
            SetupLogger();
            RegisterTypes();
            //CreateWorld();
            InitCombatSystem();
            CreateTestBots();
        }

        protected override void TearDown() {
            AILooper.Instance.Dispose();
            log.InfoFormat("------------------------Tear Down------------------------");
        }

        private void SetupLogger() {
            log4net.GlobalContext.Properties["Photon:ApplicationLogPath"] = Path.Combine(this.ApplicationRootPath, "log");
            var configFileInfo = new FileInfo(Path.Combine(this.BinaryPath, "log4net.config"));
            if (configFileInfo.Exists) {
                LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);
                XmlConfigurator.ConfigureAndWatch(configFileInfo);
            }

            log.InfoFormat("------------------------Server Started - Log Setup------------------------");
            log.Info("Assertions test.");
            Debug.Assert(0 == 1, "Assertions are turned on.");
            log.Info("Assertions test ended. Assertions are turned off if the test did not generate a log message.");
        }

        private static void RegisterTypes() {
            Photon.SocketServer.Protocol.TryRegisterCustomType(
                typeof(Vector),
                (byte)Protocol.CustomTypeCodes.Vector,
                Protocol.SerializeVector,
                Protocol.DeserializeVector);
        }

        private void CreateTestBots() {
            EntityFactory.Instance.CreateAIBot("One Punch Man", new Vector(2, 2), false);
            EntityFactory.Instance.CreateAIBot("Ork 234932", new Vector(0, -2), false);
            EntityFactory.Instance.CreateAIBot("Ork 452537", new Vector(-3, 4), false);
        }
    }
}
