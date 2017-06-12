﻿namespace JYW.ThesisMMO.MMOServer.Events {

    using Common.Codes;
    using Common.Types;
    using Photon.SocketServer.Rpc;

    class MoveEvent {

        [DataMember(Code = (byte)ParameterCode.Name)]
        public string Name;

        [DataMember(Code = (byte)ParameterCode.Position)]
        public Vector Position;
    }
}
