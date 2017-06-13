namespace JYW.ThesisMMO.UnityClient.Core.MessageHandling.Requests {

    using ExitGames.Client.Photon;
    using System.Collections.Generic;
    using JYW.ThesisMMO.Common.Codes;
    using Common.Types;
    using UnityEngine;

    public partial class RequestOperations {

        /// <summary>  
        ///  Builds the Move request end hands it to the forwarder.
        /// </summary>  
        internal static void FireStormRequest(Vector3 target) {

            var vecTarget = new Vector(target.x, target.z);

            var data = new Dictionary<byte, object>
            {
                    { (byte)ParameterCode.ActionCode, ActionCode.FireStorm },
                    { (byte)ParameterCode.Position, vecTarget }
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
