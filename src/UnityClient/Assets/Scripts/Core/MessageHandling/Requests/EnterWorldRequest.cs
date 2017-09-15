using ExitGames.Client.Photon;
using System.Collections.Generic;

namespace JYW.ThesisMMO.UnityClient.Core.MessageHandling.Requests {

    using Common.Codes;
    using JYW.ThesisMMO.UnityClient.CharacterSelection;

    public partial class RequestOperations {

        /// <summary>  
        ///  Builds the EnterWorld request end hands it to the forwarder.
        /// </summary>  
        public static void EnterWorldRequest(CharacterSetting characterSetting) {
            var data = new Dictionary<byte, object>
            {
                { (byte)ParameterCode.Name, characterSetting.Name },
                { (byte)ParameterCode.Weapon, characterSetting.Weapon},
                { (byte)ParameterCode.ActionCode, characterSetting.Skills},
                { (byte)ParameterCode.Team, characterSetting.Team }
            };

            var operationRequest = new OperationRequest() {
                OperationCode = (byte)OperationCode.EnterWorld,
                Parameters = data
            };
            RequestForwarder.ForwardRequest(
                operationRequest,
                true,
                0);
        }
    }
}
