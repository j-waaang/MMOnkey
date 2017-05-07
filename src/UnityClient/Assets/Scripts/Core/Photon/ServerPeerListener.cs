namespace JYW.ThesisMMO.UnityClient.Core.Photon {
    using System;
    using ExitGames.Client.Photon;
    using UnityEngine;
    using Common.Codes;
    using Common.Types;
    public class ServerPeerListener : IPhotonPeerListener {

        internal event Action ConnectedEvent;
        internal event Action DisconnectedEvent;
        internal event Action<Vector2> EnterWorldEvent;

        public void DebugReturn(DebugLevel level, string message) {
            Debug.Log(string.Format("{0}: {1}", level, message));
        }

        public void OnEvent(EventData eventData) {
            DebugReturn(DebugLevel.INFO, eventData.ToStringFull());
            switch ((EventCode)eventData.Code) {
                case EventCode.NewPlayer:
                    Debug.Log("Received new player event");
                    break;
            }
        }

        public void OnOperationResponse(OperationResponse operationResponse) {
            DebugReturn(DebugLevel.INFO, operationResponse.ToStringFull());
            switch ((OperationCode)operationResponse.OperationCode) {
                case OperationCode.EnterWorld:
                    if((ReturnCode) operationResponse.ReturnCode == 0) {
                        var data = (Vector) operationResponse.Parameters[(byte) ParameterCode.Position];
                        var pos = new Vector2(data.X, data.Y);
                        EnterWorldEvent(pos);
                    }
                    break;
            }
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
