namespace JYW.ThesisMMO.UnityClient.Core.MessageHandling.Responses {

    using System;
    using UnityEngine;
    using ExitGames.Client.Photon;
    using JYW.ThesisMMO.Common.Types;
    using JYW.ThesisMMO.Common.Codes;

    public sealed partial class ResponseOperations {
        public static event Action<Vector2> EnterWorldEvent;

        /// <summary>  
        ///  Extract data from a enter world response and call the enter world event.
        /// </summary>  
        private static void OnEnterWorldResponse(OperationResponse operationResponse) {
            if ((ReturnCode)operationResponse.ReturnCode == 0) {
                var data = (Vector)operationResponse.Parameters[(byte)ParameterCode.Position];
                var pos = new Vector2(data.X, data.Y);
                EnterWorldEvent(pos);
            }
        }
    }
}