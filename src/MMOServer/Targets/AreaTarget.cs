using JYW.ThesisMMO.Common.Types;

namespace JYW.ThesisMMO.MMOServer.Targets {
    abstract internal class AreaTarget : Target{

        public string SourceName { get; set; }
        public AreaTargetOption AreaTargetOption { get; set; }

        public AreaTarget() {
            TargetType = TargetType.Area;
        }

        abstract public bool IsEntityInArea(Entity entity);
    }
}
