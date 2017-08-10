using UnityEngine;
using ExitGames.Client.Photon;
using System.Collections.Generic;

namespace JYW.ThesisMMO.UnityClient.Core.MessageHandling.Requests {

    using JYW.ThesisMMO.Common.Codes;

    public partial class RequestOperations {

        /// <summary>  
        ///  Builds the Move request end hands it to the forwarder.
        /// </summary>  
        internal static void BowAutoAttackRequest(Vector3 lookDir) {

            var data = new Dictionary<byte, object>
            {
                    { (byte)ParameterCode.ActionCode, ActionCode.BowAutoAttack },
                    { (byte)ParameterCode.LookDirection, VectorExtension.Vector3ToVector(lookDir) }
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
