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
            InitializeGameTime();
            //CreateWorld();
            CreateTestBots();
            AILooper.Instance.Start();
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
            EntityFactory.Instance.CreateAIBot("One Punch Man", new Vector(6, 6), true);
            EntityFactory.Instance.CreateAIBot("Two Punch Man", new Vector(-6, -6), true);
            EntityFactory.Instance.CreateAIBot("Three Punch Man", new Vector(-6, 6), true);
            EntityFactory.Instance.CreateAIBot("Four Punch Man", new Vector(6, -6), true);
        }
    }
}
