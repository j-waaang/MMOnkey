using System;
using ExitGames.Logging;
using Photon.SocketServer.Concurrency;

namespace JYW.ThesisMMO.MMOServer {

    /// <summary>
    /// Entity notifies interest areas via regions.
    /// </summary>
    internal class Region : IDisposable {

        /// <summary>
        /// Message channel for entities moved form on channel to another.
        /// </summary>
        public MessageChannel<EntityRegionChangedMessage> EntityRegionChangedChannel { get; } =
            new MessageChannel<EntityRegionChangedMessage>(MessageCounters.CounterSend);

        /// <summary>
        /// Message channel for entities moved form on channel to another.
        /// </summary>
        public MessageChannel<InterestArea> RequestEnterRegionChannel { get; } = new MessageChannel<InterestArea>(MessageCounters.CounterSend);
        public MessageChannel<InterestArea> RequestExitRegionChannel { get; } = new MessageChannel<InterestArea>(MessageCounters.CounterSend);
        public MessageChannel<EventMessage> RegionEventChannel { get; } = new MessageChannel<EventMessage>(MessageCounters.CounterSend);

        private static readonly ILogger log = LogManager.GetCurrentClassLogger();
        private readonly BoundingBox2D m_Boundaries;

        public Region(BoundingBox2D boundingBox) {
            m_Boundaries = boundingBox;
        }

        public void Dispose() {
            EntityRegionChangedChannel.Dispose();
            RequestEnterRegionChannel.Dispose();
            RequestExitRegionChannel.Dispose();
            RegionEventChannel.Dispose();
        }

        public override string ToString() {
            return string.Format("Region({0},{1})", base.ToString(), m_Boundaries.ToString());
        }
    }
}
