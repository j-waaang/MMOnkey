namespace JYW.ThesisMMO.MMOServer.AI {

    using JYW.ThesisMMO.Common.Types;

    /// <summary> 
    /// Simple ai moving left and right.
    /// </summary>
    class TestBot : AIEntity {

        private Entity m_Entity;

        private const int MaxStepsInOneDirection = 10;
        private const float MoveSpeed = 0.2f;

        private int m_MoveDirection = 1;
        private int m_StepsDoneInOneDirection = 0;

        internal TestBot(string name, Vector position) {
            m_Entity = new Entity(name, position);
            World.Instance.AddEntity(m_Entity);
        }

        internal override void Update() {
            if (m_StepsDoneInOneDirection >= MaxStepsInOneDirection) {
                ChangeDirection();
                return;
            }

            Move();
        }

        private void Move() {
            var newPos = m_Entity.Position + new Vector(0, m_MoveDirection * MoveSpeed, 0);
            World.Instance.MoveEntity(m_Entity.Name, newPos);
            m_StepsDoneInOneDirection++;
        }

        private void ChangeDirection() {
            m_StepsDoneInOneDirection = 0;
            m_MoveDirection *= -1;
        }
    }
}
