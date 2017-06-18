using System;

namespace JYW.ThesisMMO.MMOServer.Targets {

    using JYW.ThesisMMO.Common.Types;

    class CircleAreaTarget : AreaTarget {

        public float Radius { get; set; }

        public override bool IsEntityInArea(Entity entity) {
            if(Vector.Distance(Center, entity.Position) <= Radius) {
                return true;
            }
            return false;
        }
    }
}
