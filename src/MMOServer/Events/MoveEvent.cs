namespace JYW.ThesisMMO.MMOServer.Events {

    using Common.Codes;
    using Common.Types;
    using Photon.SocketServer.Rpc;

    internal class MoveEvent {
        public MoveEvent(string name, Vector position) {
            Name = name;
            Position = position;
        }

        [DataMember(Code = (byte)ParameterCode.Name)]
        public readonly string Name;

        [DataMember(Code = (byte)ParameterCode.Position)]
        public readonly Vector Position;
    }
}
