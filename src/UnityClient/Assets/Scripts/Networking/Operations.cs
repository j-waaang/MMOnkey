namespace JYW.ThesisMMO.UnityClient.Assets.Scripts.Networking {
    using Common;
    using System;
    using System.Collections.Generic;

    internal class Operations {
        internal static void EnterWorld(Action<OperationCode, Dictionary<byte, object>, bool, byte> SendOperation, string username) {
            var data = new Dictionary<byte, object>
                {
                    { (byte)ParameterCode.Username, username }
                };
            
            SendOperation(OperationCode.EnterWorld, data, true, 0);
        }
    }
}
