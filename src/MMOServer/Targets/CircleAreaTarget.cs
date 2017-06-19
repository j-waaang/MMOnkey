using System;

namespace JYW.ThesisMMO.MMOServer.Targets {

    using Entities;
    using JYW.ThesisMMO.Common.Types;

    class CircleAreaTarget : AreaTarget {

        public float Radius { get; set; }

        public override bool IsEntityInArea(Entity entity) {
            if(entity.GetType() == typeof(SkillEntity)) { return false; }
            if(entity.Name == SourceName && AreaTargetOption == AreaTargetOption.IgnoreSource) { return false; }

            //log.InfoFormat(
            //    "AOE Center: {0}, Radius: {1}, TargetName {2}, TargetPos {3}, TargetDistance {4}",
            //    Center,
            //    Radius,
            //    entity.Name,
            //    entity.Position,
            //    Vector.Distance(Center, entity.Position)
            //    );

            if(Vector.Distance(Center, entity.Position) <= Radius) {
                return true;
            }
            return false;
        }
    }
}
