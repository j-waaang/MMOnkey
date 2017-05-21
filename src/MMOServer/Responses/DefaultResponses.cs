namespace JYW.ThesisMMO.MMOServer.Operations.Responses {
    using Common.Codes;
    using Photon.SocketServer;
    public static class DefaultResponses {
        public static OperationResponse CreateNegativeResponse(OperationRequest operationRequest, ReturnCode returnCode) {
            return new OperationResponse(operationRequest.OperationCode) {
                ReturnCode = (short)returnCode,
                DebugMessage = returnCode.ToString() + ": " + operationRequest.OperationCode.ToString()
            };
        }
    }
}
