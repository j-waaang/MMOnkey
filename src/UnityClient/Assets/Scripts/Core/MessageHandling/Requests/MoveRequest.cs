namespace JYW.ThesisMMO.UnityClient.Core.MessageHandling.Requests {

    using UnityEngine;
    using ExitGames.Client.Photon;
    using Common.Codes;
    using Common.Types;
    using System.Collections.Generic;

    public partial class RequestOperations {

        /// <summary>  
        ///  Builds the Move request end hands it to the forwarder.
        /// </summary>  
        internal static void MoveRequest(Vector2 newPosition) {
            var position = new Vector(newPosition.x, newPosition.y);

            var data = new Dictionary<byte, object>
            {
                    { (byte)ParameterCode.Position, position }
            };

            var operationRequest = new OperationRequest() {
                OperationCode = (byte)OperationCode.Move,
                Parameters = data
            };
            RequestForwarder.ForwardRequest(
                operationRequest,
                false,
                0);
        }
    }
}
