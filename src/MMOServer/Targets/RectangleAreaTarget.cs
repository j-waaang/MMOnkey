using JYW.ThesisMMO.Common.Types;

namespace JYW.ThesisMMO.MMOServer.Targets {

    /// <summary> 
    /// Rectangle defined by 3 corner points in circular order.
    /// All vectors should be Y=0.
    /// AB must be perpendicular to BC. No checks implemented.
    /// </summary>
    internal class RectangleAreaTarget : AreaTarget{

        public Vector A { get; private set; }
        public Vector B { get; private set; }
        public Vector C { get; private set; }

        private Vector m_AB;
        private Vector m_BC;
        private float m_ABAB;
        private float m_BCBC;

        public RectangleAreaTarget(Vector a, Vector b, Vector c) {
            A = a;
            B = b;
            C = c;
            m_AB = B - A;
            m_BC = C - B;
            m_ABAB = Vector.Dot(m_AB, m_AB);
            m_BCBC = Vector.Dot(m_BC, m_BC);
        }

        public override bool IsEntityInArea(Entity entity) {
            if (entity.Name == SourceName && AreaTargetOption == AreaTargetOption.IgnoreSource) { return false; }

            var M = entity.Position;

            var AM = M - A;
            var BM = M - B;

            var ABAM = Vector.Dot(m_AB, AM);
            var BCBM = Vector.Dot(m_BC, BM);

            return (
                0 < ABAM &&
                ABAM < m_ABAB &&
                0 < BCBM &&
                BCBM < m_BCBC);
        }
    }
}
