﻿using System;
using ExitGames.Logging;
using Photon.SocketServer.Concurrency;

namespace JYW.ThesisMMO.MMOServer {

    using Worlds;

    /// <summary>
    /// Entity notifies interest areas via regions.
    /// </summary>
    internal class Region : IDisposable {

        public MessageChannel<Message> EntityRegionChangedChannel { get; private set; }

        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        private BoundingBox2D m_Boundaries;

        public Region(BoundingBox2D boundingBox) {
            m_Boundaries = boundingBox;
            EntityRegionChangedChannel = new MessageChannel<Message>(MessageCounters.CounterSend);
        }

        public void Dispose() {
            EntityRegionChangedChannel.Dispose();
        }

        public override string ToString() {
            return string.Format("Region({0},{1})", base.ToString(), X, Y);
        }
    }
}
