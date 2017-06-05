namespace JYW.ThesisMMO.MMOServer.Operations.Responses {

    using Common.Codes;
    using Photon.SocketServer;
    using Photon.SocketServer.Rpc;

    public static class DefaultResponses {

        internal static OperationResponse CreateNegativeResponse(OperationRequest operationRequest, ReturnCode returnCode) {
            return new OperationResponse(operationRequest.OperationCode) {
                ReturnCode = (short)returnCode,
                DebugMessage = returnCode.ToString() + ": " + operationRequest.OperationCode.ToString()
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
