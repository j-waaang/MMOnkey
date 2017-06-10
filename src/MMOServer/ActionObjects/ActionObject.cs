namespace JYW.ThesisMMO.MMOServer.ActionObjects {

    using System.Collections.Generic;

    using Photon.SocketServer;
    using Photon.SocketServer.Rpc;

    using JYW.ThesisMMO.Common.Codes;

    /// <summary> 
    /// A game action requested by the client to change the game state.
    /// </summary>
    class ActionObject : Operation{

        protected delegate bool BoolEventHandler();

        protected event BoolEventHandler CheckPrerequisiteEvent;

        public ActionObject(IRpcProtocol protocol, OperationRequest request)
            : base(protocol, request) {
        }

        [DataMember(Code = (byte)ParameterCode.CombatActionCode)]
        public CharacterActionCode actionCode { get; set; }

        protected List<ActionContinueCondition> m_ContinueConidtions = new List<ActionContinueCondition>();

        internal bool CheckPrerequesite() {
            return false;
        }
    }
}
