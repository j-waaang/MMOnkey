﻿namespace JYW.ThesisMMO.UnityClient.Assets.Scripts.Networking {
    using ExitGames.Client.Photon;
    using System;
    using Common.Codes;
    using UnityEngine;

    public class ServerPeerListener : IPhotonPeerListener {

        internal event Action ConnectedEvent;
        internal event Action DisconnectedEvent;
        internal event Action EnterWorldEvent;

        public void DebugReturn(DebugLevel level, string message) {
            Debug.Log(string.Format("{0}: {1}", level, message));
        }

        public void OnEvent(EventData eventData) {
            DebugReturn(DebugLevel.INFO, eventData.ToStringFull());
        }

        public void OnOperationResponse(OperationResponse operationResponse) {
            DebugReturn(DebugLevel.INFO, operationResponse.ToStringFull());
            switch ((OperationCode)operationResponse.OperationCode) {
                case OperationCode.EnterWorld:
                    if((ReturnCode) operationResponse.ReturnCode == 0) {
                        EnterWorldEvent();
                    }
                    break;
                default:
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
