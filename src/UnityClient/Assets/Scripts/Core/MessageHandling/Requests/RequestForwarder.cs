// TODO: Add behaviour when peer is not connected.

namespace JYW.ThesisMMO.UnityClient.Core.MessageHandling.Requests {

    using UnityEngine;
    using ExitGames.Client.Photon;
    using Util;

    /// <summary>  
    ///  Fascades photon peer from game logic.
    /// </summary>  
    public sealed class RequestForwarder {
        private const bool m_Encrypt = false;
        private static PhotonPeer m_PhotonPeer;

        internal static void Initialize(PhotonPeer peer) {
            m_PhotonPeer = peer;
        }

        /// <summary>  
        ///  Forwards the request to the peer which then sends the request to the server.
        /// </summary>  
        internal static void ForwardRequest(OperationRequest operationRequest, bool sendReliable, byte channelId) {
            if(m_PhotonPeer == null) {
                Debug.LogError("Not connected");
                return;
            }
            m_PhotonPeer.OpCustom(operationRequest, sendReliable, channelId, m_Encrypt);
        }
    }
}