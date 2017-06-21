using JYW.ThesisMMO.Common.Types;
using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JYW.ThesisMMO.MMOServer.ActionObjects {
    internal abstract class CastActionObject : ActionObject {
        public CastActionObject(string actionSource, IRpcProtocol protocol, OperationRequest request)
                : base(actionSource, protocol, request) {
        }

        protected Vector GetLookDir(string actionSource, string actionTarget) {
            return GetLookDir(actionSource, World.Instance.GetEntity(actionTarget).Position);
        }

        protected Vector GetLookDir(string actionSource, Vector actionTarget) {
            return actionTarget - World.Instance.GetEntity(actionSource).Position;
        }
    }
}
