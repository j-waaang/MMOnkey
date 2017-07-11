namespace JYW.ThesisMMO.MMOServer {
    using ExitGames.Diagnostics.Counter;
    using Photon.SocketServer;

    /// <summary> 
    /// Stores event data and it's parameters.
    /// Used for Concurrency Channels which needs a message type.
    /// </summary>
    internal class EventMessage {

        public static readonly CountsPerSecondCounter CounterEventReceive = new CountsPerSecondCounter("Message.Receive");
        public static readonly CountsPerSecondCounter CounterEventSend = new CountsPerSecondCounter("Message.Send");

        public EventMessage(IEventData eventData, SendParameters sendParameters) {
            EventData = eventData;
            SendParameters = sendParameters;
        }

        public IEventData EventData { get; }
        internal SendParameters SendParameters { get; }
    }
}
