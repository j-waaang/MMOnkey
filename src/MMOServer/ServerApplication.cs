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
    using Properties;

    /// <summary> 
    /// Main class of the server.
    /// </summary>
    sealed class ServerApplication : ApplicationBase {

        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        protected override PeerBase CreatePeer(InitRequest initRequest) {
            return new MMOPeer(initRequest);
        }

        protected override void Setup() {
            SetupLogger();
            RegisterTypes();
            InitializeGameTime();
            CreateTestBots();

            //In eval mode we let the client start the ai loop
            if (!Settings.Default.EvaluationMode) {
                AILooper.Instance.Start();
            }
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

            //Uncomment if you want to debug the server. You can add this in any part of the code.
            //Debug.Assert(0 == 1);
        }

        private static void RegisterTypes() {
            Photon.SocketServer.Protocol.TryRegisterCustomType(
                typeof(Vector),
                (byte)Protocol.CustomTypeCodes.Vector,
                Protocol.SerializeVector,
                Protocol.DeserializeVector);
        }

        private static void InitializeGameTime() {
            // Call GameTime's static constructor.
            var i = GameTime.Time;
        }

        private void CreateTestBots() {
            switch (Settings.Default.EntityScenario) {
                case 0:
                    EntityScenario.CreateHomogeneousScenario();
                    break;
                case 1:
                    EntityScenario.CreateCentralizedScenario();
                    break;
                case 2:
                    EntityScenario.CreateSingleEnemy();
                    break;
            }
        }
    }
}
