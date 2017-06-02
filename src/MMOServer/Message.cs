namespace JYW.ThesisMMO.MMOServer {

    using Photon.SocketServer;

    /// <summary> 
    /// Stores event data and it's parameters in on struct.
    /// Used for Concurrency Channels which needs a message type.
    /// </summary>
    internal struct Message {

        internal Message(IEventData eventData, SendParameters sendParameters) {
            this.eventData = eventData;
            this.sendParameters = sendParameters;
        }

        internal IEventData eventData;
        internal SendParameters sendParameters;
    }
}
