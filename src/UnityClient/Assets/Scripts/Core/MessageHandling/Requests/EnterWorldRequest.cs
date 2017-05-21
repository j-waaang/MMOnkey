namespace JYW.ThesisMMO.UnityClient.Core.MessageHandling.Requests {

    using ExitGames.Client.Photon;
    using Common.Codes;
    using System.Collections.Generic;
    using JYW.ThesisMMO.UnityClient.CharacterSelection;

    public partial class RequestOperations {

        /// <summary>  
        ///  Builds the EnterWorld request end hands it to the forwarder.
        /// </summary>  
        public static void EnterWorldRequest(CharacterSetting characterSetting) {
            var data = new Dictionary<byte, object>
            {
                { (byte)ParameterCode.Username, characterSetting.Name },
                { (byte)ParameterCode.Weapon, characterSetting.Weapon},
                { (byte)ParameterCode.Skill, characterSetting.Skills}
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
