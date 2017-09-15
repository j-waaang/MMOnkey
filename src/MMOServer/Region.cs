using System;
using ExitGames.Logging;
using Photon.SocketServer.Concurrency;

namespace JYW.ThesisMMO.MMOServer {

    /// <summary>
    /// Entity notifies interest areas via regions.
    /// </summary>
    internal class Region : IDisposable, IEquatable<Region> {

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

        /// <summary>
        /// All IAs can publish the changes of it's client to this channel.
        /// Only Client IA's sub to this channel.
        /// </summary>
        public MessageChannel<Entity> PositionUpdateChannel { get; } =
            new MessageChannel<Entity>(MessageCounters.CounterSend);

        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Used for debugging only.
        /// </summary>
        public BoundingBox2D Boundaries { get; }
        public int X { get; }
        public int Z { get; }

        public Region(BoundingBox2D boundingBox, int x, int z) {
            Boundaries = boundingBox;
            X = x;
            Z = z;
        }

        public void Dispose() {
            EntityRegionChangedChannel.Dispose();
            RequestInfoInRegionChannel.Dispose();
            RequestRegionExitInfoChannel.Dispose();
            RegionEventChannel.Dispose();
        }

        public override string ToString() {
            return string.Format("Tile: ({0},{1}), Bounds: {2}", X, Z, Boundaries.ToString());
        }

        public bool Equals(Region other) {
            return X == other.X && Z == other.Z;
        }
    }
}
