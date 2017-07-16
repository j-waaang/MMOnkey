namespace JYW.ThesisMMO.MMOServer {
    using Events.ActionEvents;
    using ExitGames.Diagnostics.Counter;
    using Photon.SocketServer;

    /// <summary> 
    /// Stores event data and it's parameters.
    /// Used for Concurrency Channels which needs a message type.
    /// </summary>
    internal class EventMessage {

        public static readonly CountsPerSecondCounter CounterEventReceive = new CountsPerSecondCounter("Message.Receive");
        public static readonly CountsPerSecondCounter CounterEventSend = new CountsPerSecondCounter("Message.Send");

        public EventMessage(IEventData eventData, SendParameters sendParameters, BroadcastOptions broadcastOptions, string sender) {
            this.eventData = eventData;
            this.sendParameters = sendParameters;
            this.broadcastOptions = broadcastOptions;
            this.sender = sender;
        }

        internal readonly string sender;
        internal readonly IEventData eventData;
        internal readonly SendParameters sendParameters;
        internal readonly BroadcastOptions broadcastOptions;
    }
}
