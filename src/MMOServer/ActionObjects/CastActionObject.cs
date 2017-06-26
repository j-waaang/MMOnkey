using JYW.ThesisMMO.Common.Codes;
using JYW.ThesisMMO.Common.ContinueObjects;
using JYW.ThesisMMO.Common.ContinueObjects.InteruptObjects;
using JYW.ThesisMMO.Common.Types;
using JYW.ThesisMMO.MMOServer.Entities.Attributes.Modifiers;
using Photon.SocketServer;
using System;

namespace JYW.ThesisMMO.MMOServer.ActionObjects {
    internal abstract class CastActionObject : ActionObject {

        protected static InteruptionHandler m_InteruptionHandler = new InteruptionHandler();
        protected event Action<CallReason> FinishedCastingEvent;

        static CastActionObject() {
            InteruptContinueCondition.SetInteruptionHandler(m_InteruptionHandler);
        }

        public CastActionObject(string actionSource, IRpcProtocol protocol, OperationRequest request)
                : base(actionSource, protocol, request) {
        }

        protected void StartCast(TimeSpan castDuration, ActionCode castState, Vector lookDirection) {
            var stateModifier = new CastActionStateModifier(castState, lookDirection);
            World.Instance.ApplyModifier(ActionSource, stateModifier);
            AddCondition(new TimedContinueCondition(castDuration));
            AddCondition(new InteruptContinueCondition(ActionSource));

            ContinueEvent += FinishedCastingEvent;
            StartConditions();
        }

        protected Vector GetLookDir(string actionSource, string actionTarget) {
            return GetLookDir(actionSource, World.Instance.GetEntity(actionTarget).Position);
        }

        protected Vector GetLookDir(string actionSource, Vector actionTarget) {
            return actionTarget - World.Instance.GetEntity(actionSource).Position;
        }
    }
}
