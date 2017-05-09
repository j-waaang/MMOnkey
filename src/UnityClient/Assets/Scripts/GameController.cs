namespace JYW.ThesisMMO.UnityClient {

    using UnityEngine;
    using JYW.ThesisMMO.UnityClient.Core.MessageHandling.Requests;

    public sealed class GameController : MonoBehaviour {

        private void Awake() {
            NotifyFinishedLoadingWorld();
        }

        /// <summary>  
        ///  Notify the server that the scene finished loading and we are ready to receive game actions.
        /// </summary>  
        private static void NotifyFinishedLoadingWorld() {
            RequestOperations.ReadyToReceiveGameEventsRequest();
        }
    }
}