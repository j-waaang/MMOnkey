using System.Collections.Generic;
using JYW.ThesisMMO.Common.GameWorld.Entities;

namespace JYW.ThesisMMO.Common.GameWorld {
    class TestMap{
        public readonly BoundingBox m_Boundary;
        private List<Entity> m_Entities;

        public TestMap() {
            m_Boundary = new BoundingBox(50f, 50f);
        }
    }
}
