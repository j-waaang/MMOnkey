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
    using System;
    using System.Reflection;

    /// <summary> 
    /// Main class of the server.
    /// </summary>
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

            CreateTestBots();
            //TestFireStorm();
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
            log.DebugFormat("Created Game World.");
        }

        private void StartAIModule() {
            m_AIModule = new AIModule();
            m_AIModule.Start();
        }

        protected override void TearDown() {
            m_AIModule.Stop();
            log.DebugFormat("Tear Down");
        }

        private void CreateTestBots() {
            m_AIModule.AddEntity(new TestBot("one", new Vector(2, 2)));
            m_AIModule.AddEntity(new TestBot("two", new Vector(0, -2)));
            m_AIModule.AddEntity(new TestBot("three", new Vector(-3, 4)));
        }

        private void TestFireStorm() {
            log.Debug("------------------------Test reflection------------------------");
            log.Debug("Listing all classes");

            Assembly thisType = GetType().Assembly;
            foreach (Type type in thisType.GetTypes()) {
                log.Debug(type.FullName);
            }
            var namespacke = "JYW.ThesisMMO.MMOServer.ActionObjects.SkillRequests.";
            var stringType = namespacke + Common.Codes.ActionCode.FireStorm.ToString() + "Request";
            log.DebugFormat("Looking for type {0}", stringType);
            var fsType = Type.GetType(stringType);
            if(fsType == null) { log.Debug("Type not found."); }
            Activator.CreateInstance(fsType, "mofofofoosso");
        }
    }
}
