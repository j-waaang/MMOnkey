namespace JYW.ThesisMMO.UnityClient.Core.MessageHandling.Responses {

    using UnityEngine;
    using ExitGames.Client.Photon;
    using JYW.ThesisMMO.Common.Codes;

    public sealed partial class ResponseOperations {
        internal static void OnOperationResponse(OperationResponse operationResponse) {
            switch ((OperationCode)operationResponse.OperationCode) {
                case OperationCode.EnterWorld:
                    OnEnterWorldResponse(operationResponse);
                    return;
            }

            Debug.LogError("Cannot handle response." + operationResponse.DebugMessage);
        }
    }
}
