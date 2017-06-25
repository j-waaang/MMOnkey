using System;
using JYW.ThesisMMO.Common.Types;

namespace JYW.ThesisMMO.MMOServer.Targets {
    internal class Cone2DAreaTarget : AreaTarget {

        public Vector Center { get; private set; }
        public Vector LookDirection { get; private set; }
        public float Angle { get; private set; }
        public float Radius { get; private set; }

        private float m_MaxOtherRad;
        private readonly float bLen;

        public Cone2DAreaTarget(Vector center, Vector lookDir, float angleDegree, float radius) {
            Center = center;
            LookDirection = lookDir;
            Angle = angleDegree;
            Radius = radius;
            m_MaxOtherRad = Deg2Rad(angleDegree) * 0.5f;
            bLen = LookDirection.Length;
        }

        public override bool IsEntityInArea(Entity entity) {
            if (!DefaultCheck(entity)) { return false; }
            if (Vector.Distance(Center, entity.Position) > Radius) { return false; }

            float aLen = Vector.Distance(entity.Position, Center + LookDirection);
            float cLen = Vector.Distance(Center, entity.Position);

            // Determine angle of a with triangle sides a,b,c using cosine rule.
            var aRad =
                (aLen * aLen - bLen * bLen - cLen * cLen)
                /
                (-2f * bLen * cLen);

            aRad = (float)Math.Acos(aRad);
            if (m_MaxOtherRad < aRad) { return false; }

            return true;
        }

        private static float Deg2Rad(float angle) {
            return (float)Math.PI * angle / 180f;
        }

        private static float Rad2Deg(float angle) {
            return angle * (180f / (float)Math.PI);
        }
    }
}
