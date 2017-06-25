using JYW.ThesisMMO.Common.Types;
using JYW.ThesisMMO.MMOServer.Entities;

namespace JYW.ThesisMMO.MMOServer.Targets {
    abstract internal class AreaTarget : Target{

        public string SourceName { get; set; }
        public AreaTargetOption AreaTargetOption { get; set; }

        public AreaTarget() {
            TargetType = TargetType.Area;
        }

        abstract public bool IsEntityInArea(Entity entity);

        protected bool DefaultCheck(Entity entity) {
            if (entity.GetType() == typeof(SkillEntity)) { return false; }
            if (entity.Name == SourceName && AreaTargetOption == AreaTargetOption.IgnoreSource) { return false; }

            return true;
        }
    }
}
