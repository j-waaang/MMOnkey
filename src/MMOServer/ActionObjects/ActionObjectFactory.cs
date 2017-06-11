namespace JYW.ThesisMMO.MMOServer.ActionObjects {

    using JYW.ThesisMMO.Common.Codes;
    using Photon.SocketServer;

    /// <summary> 
    /// Creates ActionObjects.
    /// </summary>
    internal class ActionObjectFactory {

        internal ReturnCode LastCreationFailReason { get; private set; }

        internal ActionObject CreateActionObject(string actionSource, IRpcProtocol protocol, OperationRequest request) {
            var action = (ActionCode)request.Parameters[(byte)ParameterCode.ActionCode];
            ActionObject actionObject = null;
            switch (action) {
                case ActionCode.AxeAutoAttack:
                    actionObject = new AxeAutoAttackRequest(actionSource, protocol, request);
                    break;
                case ActionCode.BowAutoAttack:
                    actionObject = new BowAutoAttackRequest(protocol, request);
                    break;
                case ActionCode.Move:
                case ActionCode.Dash:
                case ActionCode.DistractingShot:
                case ActionCode.FireStorm:
                case ActionCode.HammerBash:
                case ActionCode.OrisonOfHealing:
                default:
                    LastCreationFailReason = ReturnCode.OperationNotImplemented;
                    break;
            }

            return actionObject;
        }
    }
}
