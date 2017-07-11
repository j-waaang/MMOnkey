using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JYW.ThesisMMO.MMOServer {
    class ClientInterestArea : InterestArea {
        public ClientInterestArea(Entity attachedEntity, float interestRadius) : base(attachedEntity, interestRadius) {
        }
    }
}
