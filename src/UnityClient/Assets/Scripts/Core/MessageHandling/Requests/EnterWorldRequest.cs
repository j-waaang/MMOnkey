namespace JYW.ThesisMMO.UnityClient.Core.MessageHandling {

    using ExitGames.Client.Photon;
    using Common.Codes;
    using System.Collections.Generic;

    public partial class RequestOperations {
        /// <summary>  
        ///  Builds the EnterWorld request end hands it to the forwarder.
        /// </summary>  
        internal static void EnterWorld(string username) {
            var data = new Dictionary<byte, object>
            {
                    { (byte)ParameterCode.Username, username }
            };

            var operationRequest = new OperationRequest() {
                OperationCode = (byte)OperationCode.EnterWorld,
                Parameters = data
            };
            RequestForwarder.Instance.ForwardRequest(
                operationRequest,
                true,
                0);
        }
    }
}
