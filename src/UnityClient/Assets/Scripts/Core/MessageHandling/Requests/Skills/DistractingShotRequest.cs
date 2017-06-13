namespace JYW.ThesisMMO.UnityClient.Core.MessageHandling.Requests {

    using ExitGames.Client.Photon;
    using System.Collections.Generic;
    using JYW.ThesisMMO.Common.Codes;

    public partial class RequestOperations {

        /// <summary>  
        ///  Builds the Move request end hands it to the forwarder.
        /// </summary>  
        internal static void DistractingShotRequest(string target) {

            var data = new Dictionary<byte, object>
            {
                    { (byte)ParameterCode.ActionCode, ActionCode.DistractingShot },
                    { (byte)ParameterCode.Name, target }
            };

            var operationRequest = new OperationRequest() {
                OperationCode = (byte)OperationCode.CharacterAction,
                Parameters = data
            };

            RequestForwarder.ForwardRequest(
                operationRequest,
                true,
                0);
        }
    }
}
