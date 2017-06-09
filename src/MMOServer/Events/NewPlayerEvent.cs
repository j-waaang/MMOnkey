﻿namespace JYW.ThesisMMO.MMOServer.Events {

    using Common.Codes;
    using Common.Types;
    using Photon.SocketServer.Rpc;

    class NewPlayerEvent {

        [DataMember(Code = (byte)ParameterCode.Name)]
        public string Username;

        [DataMember(Code = (byte)ParameterCode.Position)]
        public Vector Position;
    }
}
