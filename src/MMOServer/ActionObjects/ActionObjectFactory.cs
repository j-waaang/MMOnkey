namespace JYW.ThesisMMO.MMOServer.ActionObjects {

    using JYW.ThesisMMO.Common.Codes;
    using JYW.ThesisMMO.MMOServer.ActionObjects.SkillRequests;
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
                    actionObject = new BowAutoAttackRequest(actionSource, protocol, request);
                    break;
                case ActionCode.Dash:
                    actionObject = new DashRequest(actionSource, protocol, request);
                    break;
                case ActionCode.OrisonOfHealing:
                    actionObject = new OrisonOfHealingRequest(actionSource, protocol, request);
                    break;
                case ActionCode.DistractingShot:
                case ActionCode.FireStorm:
                case ActionCode.HammerBash:
                default:
                    LastCreationFailReason = ReturnCode.OperationNotImplemented;
                    break;
            }

            return actionObject;
        }
    }
}
