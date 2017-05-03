namespace JYW.ThesisMMO.MMOServer {
    using Photon.SocketServer;
    internal struct Message {
        internal Message(IEventData eventData, SendParameters sendParameters) {
            this.eventData = eventData;
            this.sendParameters = sendParameters;
        }
        internal IEventData eventData;
        internal SendParameters sendParameters;
    }
}
