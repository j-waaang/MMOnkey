namespace JYW.ThesisMMO.MMOServer.Targets {

    using JYW.ThesisMMO.Common.Types;

    internal class CircleAreaTarget : AreaTarget {

        public float Radius { get; private set; }
        public Vector Center { get; private set; }

        public CircleAreaTarget(Vector center, float radius) {
            Radius = radius;
            Center = center;
        }

        public override bool IsEntityInArea(Entity entity) {
            if (!DefaultCheck(entity)) { return false; }
            if (Vector.Distance(Center, entity.Position) > Radius) { return false; }

            return true;
        }
    }
}
