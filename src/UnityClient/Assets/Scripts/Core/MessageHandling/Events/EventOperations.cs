namespace JYW.ThesisMMO.UnityClient.Core.MessageHandling.Events {

    using UnityEngine;
    using Common.Codes;
    using ExitGames.Client.Photon;

    public sealed partial class EventOperations {
        internal static void OnEvent(EventData eventData) {
            switch ((EventCode)eventData.Code) {
                case EventCode.NewPlayer:
                    OnNewPlayerEvent(eventData);
                    return;
            }
            Debug.LogError("Cannot handle response.");
        }
    }
}
