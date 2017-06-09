namespace JYW.ThesisMMO.MMOServer.CombatActions {

    using Common.Codes;
    using Operations.Responses;
    using Photon.SocketServer;
    using Photon.SocketServer.Rpc;
    using Requests;
    using System.Collections.Generic;

    /// <summary> 
    /// Combat actions must pass this before beeing send to game world.
    /// </summary>
    internal class CombatActionManager {
        internal static CombatActionManager Instance { get; private set; }

        private ActionObjectFactory m_ActionObjectFactory = new ActionObjectFactory();
        private List<ActionObject> m_ActionObjects = new List<ActionObject>();

        internal CombatActionManager() {
            if (Instance == null) {
                Instance = this;
            }
            else { return; }
        }

        internal OperationResponse CombatActionRequest(PeerBase peer, OperationRequest request, SendParameters sendParameters) {
            var action = (CharacterActionCode)request.Parameters[(byte)ParameterCode.CombatActionCode];
            CharacterActionRequest operation = null;
            switch (action) {
                case CharacterActionCode.AxeAutoAttack:
                    operation = new AxeAutoAttackRequest(peer.Protocol, request);
                    break;
                case CharacterActionCode.BowAutoAttack:
                    operation = new BowAutoAttackRequest(peer.Protocol, request);
                    break;
                case CharacterActionCode.Move:
                    break;
                case CharacterActionCode.Dash:
                    break;
                case CharacterActionCode.DistractingShot:
                    break;
                case CharacterActionCode.FireStorm:
                    break;
                case CharacterActionCode.HammerBash:
                    break;
                case CharacterActionCode.OrisonOfHealing:
                    break;
                default:
                    break;
            }

            if (!operation.IsValid) { return DefaultResponses.CreateInvalidOperationParameterResponse(operation); }

            m_ActionObjectFactory.CreateActionObject(action, operation);

            return null;
        }
    }
}
