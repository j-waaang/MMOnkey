using System;
using ExitGames.Client.Photon;
using UnityEngine;
using System.Collections.Generic;

namespace JYW.ThesisMMO.UnityClient.Core.MessageHandling.Responses {

    using JYW.ThesisMMO.Common.Codes;

    public sealed partial class ResponseOperations {

        public static Dictionary<ActionCode, Action> ActionAcceptanceWaitinglist = new Dictionary<ActionCode, Action>();

        public static void AddActionToResponseWaitinglist(ActionCode key, Action value) {
            ActionAcceptanceWaitinglist.Add(key, value);
        }

        /// <summary>  
        ///  Extract data from a enter world response and call the enter world event.
        /// </summary>  
        private static void OnCharacterActionResponse(OperationResponse operationResponse) {
            var code = (ActionCode)operationResponse.Parameters[(byte)ParameterCode.ActionCode];

            switch ((ReturnCode)operationResponse.ReturnCode) {
                case ReturnCode.OK:
                    ActionAcceptanceWaitinglist[code]();
                    ActionAcceptanceWaitinglist.Remove(code);
                    break;
                case ReturnCode.Declined:
                    ActionAcceptanceWaitinglist.Remove(code);
                    break;
            }
        }
    }
}