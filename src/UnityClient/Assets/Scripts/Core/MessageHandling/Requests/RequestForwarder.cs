// TODO: Add behaviour when peer is not connected.

namespace JYW.ThesisMMO.UnityClient.Core.MessageHandling.Requests {
    using ExitGames.Client.Photon;
    using Util;

    /// <summary>  
    ///  Fascades photon peer from game logic.
    /// </summary>  
    public sealed class RequestForwarder : Singleton<RequestForwarder>{
        private const bool m_Encrypt = false;
        private PhotonPeer m_PhotonPeer;

        internal RequestForwarder(PhotonPeer peer) {
            Instance = this;
            m_PhotonPeer = peer;
        }

        /// <summary>  
        ///  Forwards the request to the peer which then sends the request to the server.
        /// </summary>  
        public void ForwardRequest(OperationRequest operationRequest, bool sendReliable, byte channelId) {
            m_PhotonPeer.OpCustom(operationRequest, sendReliable, channelId, m_Encrypt);
        }
    }
}