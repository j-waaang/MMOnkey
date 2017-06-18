namespace JYW.ThesisMMO.MMOServer.ActionObjects {
    using ExitGames.Logging;
    using JYW.ThesisMMO.Common.Codes;
    using JYW.ThesisMMO.MMOServer.ActionObjects.SkillRequests;
    using Photon.SocketServer;
    using System;

    /// <summary> 
    /// Creates ActionObjects.
    /// </summary>
    internal class ActionObjectFactory {

        private const string skillNamespace = "JYW.ThesisMMO.MMOServer.ActionObjects.SkillRequests.";

        protected static readonly ILogger log = LogManager.GetCurrentClassLogger();

        internal ReturnCode LastCreationFailReason { get; private set; }

        internal ActionObject CreateActionObject(string actionSource, IRpcProtocol protocol, OperationRequest request) {
            var action = (ActionCode)request.Parameters[(byte)ParameterCode.ActionCode];

            var stringType = skillNamespace + action + "Request";
            var actionType = Type.GetType(stringType);

            if(actionType == null) {
                log.DebugFormat("Type {0} was not found.", stringType);
                LastCreationFailReason = ReturnCode.OperationNotImplemented;
                return null;
            }

            return Activator.CreateInstance(actionType, actionSource, protocol, request) as ActionObject;

            //ActionObject actionObject = null;
            //switch (action) {
            //    case ActionCode.AxeAutoAttack:
            //        actionObject = new AxeAutoAttackRequest(actionSource, protocol, request);
            //        break;
            //    case ActionCode.BowAutoAttack:
            //        actionObject = new BowAutoAttackRequest(actionSource, protocol, request);
            //        break;
            //    case ActionCode.Dash:
            //        actionObject = new DashRequest(actionSource, protocol, request);
            //        break;
            //    case ActionCode.OrisonOfHealing:
            //        actionObject = new OrisonOfHealingRequest(actionSource, protocol, request);
            //        break;
            //    case ActionCode.DistractingShot:
            //    case ActionCode.FireStorm:
            //    case ActionCode.HammerBash:
            //    default:
            //        LastCreationFailReason = ReturnCode.OperationNotImplemented;
            //        break;
            //}

            //return actionObject;
        }
    }
}
