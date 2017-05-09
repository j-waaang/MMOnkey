namespace JYW.ThesisMMO.UnityClient.Core.MessageHandling.Requests {

    using ExitGames.Client.Photon;
    using Common.Codes;
    using System.Collections.Generic;

    public partial class RequestOperations {

        /// <summary>  
        ///  Builds the FinishedLoadingWorld request end hands it to the forwarder.
        /// </summary>  
        public static void ReadyToReceiveGameEventsRequest() {
            var operationRequest = new OperationRequest() {
                OperationCode = (byte)OperationCode.ReadyToReceiveGameEvents,
            };
            RequestForwarder.ForwardRequest(
                operationRequest,
                true,
                0);
        }
    }
}
