namespace JYW.ThesisMMO.UnityClient.Core.Photon {
    using System;
    using ExitGames.Client.Photon;
    using UnityEngine;
    using JYW.ThesisMMO.UnityClient.Core.MessageHandling.Responses;
    using JYW.ThesisMMO.UnityClient.Core.MessageHandling.Events;
    public class ServerPeerListener : IPhotonPeerListener {

        internal event Action ConnectedEvent;
        internal event Action DisconnectedEvent;

        public void DebugReturn(DebugLevel level, string message) {
            //Debug.Log(string.Format("{0}: {1}", level, message));
        }

        public void OnEvent(EventData eventData) {
            DebugReturn(DebugLevel.INFO, eventData.Code.ToString() + eventData.ToStringFull());
            EventOperations.OnEvent(eventData);
        }

        public void OnOperationResponse(OperationResponse operationResponse) {
            DebugReturn(DebugLevel.INFO, operationResponse.ToStringFull());
            ResponseOperations.OnOperationResponse(operationResponse);
        }

        public void OnStatusChanged(StatusCode statusCode) {
            switch (statusCode) {
                case StatusCode.Connect:
                    DebugReturn(DebugLevel.INFO, "Connected");
                    if (ConnectedEvent != null) { ConnectedEvent(); }
                    break;
                case StatusCode.Disconnect:
                case StatusCode.DisconnectByServer:
                case StatusCode.DisconnectByServerLogic:
                case StatusCode.DisconnectByServerUserLimit:
                    DebugReturn(DebugLevel.INFO, "Disconnected");
                    if(DisconnectedEvent != null) { DisconnectedEvent(); }
                    break;
                default:
                    DebugReturn(DebugLevel.ERROR, statusCode.ToString());
                    break;
            }
        }
    }
}
