using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JYW.ThesisMMO.MMOServer {
    class ClientInterestArea : InterestArea {
        public ClientInterestArea(Entity attachedEntity, float interestRadius) : base(attachedEntity, interestRadius) {
        }

        /// <summary>
        /// Subs and unsubs from regions depending on focus.
        /// Should be called when entering a new region or the entity moved.
        /// </summary>
        public override void UpdateRegionSubscription() {

            var focusedRegions = World.Instance.GetRegions(m_Focus);
            log.InfoFormat("{0} focused {1} regions.", m_AttachedEntity.Name, focusedRegions.Count());

            SubscribeRegions(focusedRegions);
            UnsubscribeRegionsNotIn(focusedRegions);

            string subbedregions = "";
            foreach (var region in m_Regions) {
                subbedregions += region.ToString() + "/n";
            }

            log.InfoFormat("{0} subed to {2} regions {1}", m_AttachedEntity.Name, subbedregions, subbedregions.Count());
        }
    }
}
