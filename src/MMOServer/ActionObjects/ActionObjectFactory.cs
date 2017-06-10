namespace JYW.ThesisMMO.MMOServer.ActionObjects {

    using JYW.ThesisMMO.Common.Codes;
    using Photon.SocketServer;

    /// <summary> 
    /// Creates ActionObjects.
    /// </summary>
    internal class ActionObjectFactory {

        internal ReturnCode LastCreationFailReason { get; private set; }

        internal ActionObject CreateActionObject(IRpcProtocol protocol, OperationRequest request) {
            var action = (CharacterActionCode)request.Parameters[(byte)ParameterCode.CombatActionCode];
            ActionObject actionObject = null;
            switch (action) {
                case CharacterActionCode.AxeAutoAttack:
                    actionObject = new AxeAutoAttackRequest(protocol, request);
                    break;
                case CharacterActionCode.BowAutoAttack:
                    actionObject = new BowAutoAttackRequest(protocol, request);
                    break;
                case CharacterActionCode.Move:
                case CharacterActionCode.Dash:
                case CharacterActionCode.DistractingShot:
                case CharacterActionCode.FireStorm:
                case CharacterActionCode.HammerBash:
                case CharacterActionCode.OrisonOfHealing:
                default:
                    LastCreationFailReason = ReturnCode.OperationNotImplemented;
                    break;
            }

            return actionObject;
        }
    }
}
