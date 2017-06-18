namespace JYW.ThesisMMO.MMOServer.AI {

    using JYW.ThesisMMO.Common.Types;

    /// <summary> 
    /// Simple ai moving left and right.
    /// </summary>
    internal class TestBot : AIEntity {

        private const int MaxStepsInOneDirection = 10;
        private const float MoveSpeed = 0.2f;

        private int m_MoveDirection = 1;
        private int m_StepsDoneInOneDirection = 0;

        public TestBot(Entity entity) : base (entity) {
        }

        public override void Update() {
            if (m_StepsDoneInOneDirection >= MaxStepsInOneDirection) {
                ChangeDirection();
                return;
            }

            Move();
        }

        private void Move() {
            var newPos = m_Entity.Position + new Vector(0, m_MoveDirection * MoveSpeed);
            World.Instance.MoveEntity(m_Entity.Name, newPos);
            m_StepsDoneInOneDirection++;
        }

        private void ChangeDirection() {
            m_StepsDoneInOneDirection = 0;
            m_MoveDirection *= -1;
        }
    }
}
