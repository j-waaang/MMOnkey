namespace JYW.ThesisMMO.MMOServer {
    using ExitGames.Diagnostics.Counter;
    using Photon.SocketServer;

    /// <summary> 
    /// Stores event data and it's parameters in on struct.
    /// Used for Concurrency Channels which needs a message type.
    /// </summary>
    internal struct EventMessage {

        public static readonly CountsPerSecondCounter CounterEventReceive = new CountsPerSecondCounter("Message.Receive");
        public static readonly CountsPerSecondCounter CounterEventSend = new CountsPerSecondCounter("Message.Send");

        internal EventMessage(IEventData eventData, SendParameters sendParameters) {
            this.eventData = eventData;
            this.sendParameters = sendParameters;
        }

        internal IEventData eventData;
        internal SendParameters sendParameters;
    }
}
