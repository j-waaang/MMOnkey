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
                case OperationCode.Move:
                case OperationCode.CharacterAction:
                    Debug.LogFormat(
                        "Received {0} from {1} response.",
                        (ReturnCode)operationResponse.ReturnCode,
                        (OperationCode)operationResponse.OperationCode);
                    return;
                default:
                    Debug.LogError("Cannot handle response." + operationResponse.DebugMessage);
                    break;
            }
        }
    }
}
