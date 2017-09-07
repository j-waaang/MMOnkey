using ExitGames.Client.Photon;

namespace JYW.ThesisMMO.UnityClient.Core.MessageHandling.Requests {

    using Common.Codes;
    using JYW.ThesisMMO.UnityClient.CharacterSelection;

    public partial class RequestOperations {

        /// <summary>  
        ///  Send request to server to start Ai Loop.
        /// </summary>  
        public static void StartAIRequest() {

            var operationRequest = new OperationRequest() {
                OperationCode = (byte)OperationCode.StartAiLoop
            };
            RequestForwarder.ForwardRequest(
                operationRequest,
                true,
                0);
        }

        /// <summary>  
        ///  Send request to server to toggle Ai Loop.
        /// </summary>  
        public static void ToggleAiLoopRequest() {

            var operationRequest = new OperationRequest() {
                OperationCode = (byte)OperationCode.ToggleAiLoop
            };
            RequestForwarder.ForwardRequest(
                operationRequest,
                true,
                0);
        }
    }
}
