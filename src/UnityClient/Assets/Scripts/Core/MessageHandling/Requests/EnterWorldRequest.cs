﻿namespace JYW.ThesisMMO.UnityClient.Core.MessageHandling.Requests {

    using ExitGames.Client.Photon;
    using Common.Codes;
    using System.Collections.Generic;

    public partial class RequestOperations {

        /// <summary>  
        ///  Builds the EnterWorld request end hands it to the forwarder.
        /// </summary>  
        public static void EnterWorldRequest(string username) {
            var data = new Dictionary<byte, object>
            {
                    { (byte)ParameterCode.Username, username }
            };

            var operationRequest = new OperationRequest() {
                OperationCode = (byte)OperationCode.EnterWorld,
                Parameters = data
            };
            RequestForwarder.ForwardRequest(
                operationRequest,
                true,
                0);
        }
    }
}
