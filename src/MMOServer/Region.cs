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
        /// Interest areas subscribed to this channel sends a snapshot of it's entity to the publisher.
        /// Client IAs sub and pub to this channel.
        /// Other IAs only sub to this channel.
        /// </summary>
        public MessageChannel<InterestArea> RequestInfoInRegionChannel { get; } =
            new MessageChannel<InterestArea>(MessageCounters.CounterSend);

        /// <summary>
        /// Interest areas subscribed to this channel sends it's entity's name to the publisher.
        /// Client IAs sub and pub to this channel.
        /// Other IAs only sub to this channel.
        /// </summary>
        public MessageChannel<InterestArea> RequestRegionExitInfoChannel { get; } =
            new MessageChannel<InterestArea>(MessageCounters.CounterSend);

        /// <summary>
        /// All IAs can publish the changes of it's client to this channel.
        /// Only Client IA's sub to this channel.
        /// </summary>
        public MessageChannel<EventMessage> RegionEventChannel { get; } =
            new MessageChannel<EventMessage>(MessageCounters.CounterSend);

        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Used for debugging only.
        /// </summary>
        public BoundingBox2D Boundaries { get; }

        public Region(BoundingBox2D boundingBox) {
            Boundaries = boundingBox;
        }

        public void Dispose() {
            EntityRegionChangedChannel.Dispose();
            RequestInfoInRegionChannel.Dispose();
            RequestRegionExitInfoChannel.Dispose();
            RegionEventChannel.Dispose();
        }

        public override string ToString() {
            return string.Format("Region {0}", Boundaries.ToString());
        }
    }
}
