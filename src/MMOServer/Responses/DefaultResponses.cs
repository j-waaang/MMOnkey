using Photon.SocketServer;
using Photon.SocketServer.Rpc;
using System.Collections.Generic;

namespace JYW.ThesisMMO.MMOServer.Operations.Responses {

    using Common.Codes;

    public static class DefaultResponses {

        internal static OperationResponse CreateNegativeResponse(OperationRequest operationRequest, ReturnCode returnCode) {
            return new OperationResponse(operationRequest.OperationCode) {
                ReturnCode = (short)returnCode,
                DebugMessage = returnCode.ToString() + ": " + operationRequest.OperationCode.ToString()
            };
        }

        internal static OperationResponse CreateActionRequestResponse(OperationRequest operationRequest, ReturnCode returnCode) {
            var paramCode = (byte)ParameterCode.ActionCode;
            
            return new OperationResponse(operationRequest.OperationCode) {
                ReturnCode = (short)returnCode,
                Parameters = new Dictionary<byte, object>() {
                    { paramCode, operationRequest.Parameters[paramCode] }
                }
            };
        }

        internal static OperationResponse CreateInvalidOperationParameterResponse(Operation operation) {
            return new OperationResponse(operation.OperationRequest.OperationCode) {
                ReturnCode = (int)ReturnCode.InvalidOperationParameter,
                DebugMessage = operation.GetErrorMessage()
            };
        }
    }
}
