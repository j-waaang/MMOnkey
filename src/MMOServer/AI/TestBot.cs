using System;

namespace JYW.ThesisMMO.MMOServer.AI {

    using JYW.ThesisMMO.Common.Types;

    /// <summary> 
    /// Simple ai moving left and right.
    /// </summary>
    internal class TestBot : AIEntity {

        public bool canMove = true;

        private const int MaxStepsInOneDirection = 30;
        private const float MoveSpeed = 7f;

        private int m_MoveDirection = 1;
        private int m_StepsDoneInOneDirection = 0;

        public TestBot(Entity entity) : base (entity) {
        }

        public override void Update(TimeSpan deltaTime) {
            if (!canMove) { return; }

            if (m_StepsDoneInOneDirection >= MaxStepsInOneDirection) {
                ChangeDirection();
                return;
            }
            Move(deltaTime);
        }

        private void Move(TimeSpan deltaTime) {
            var moveAmt = new Vector(0, m_MoveDirection * MoveSpeed * (float)deltaTime.TotalSeconds);
            var newPos = Entity.Position + moveAmt;
            World.Instance.MoveEntity(Entity.Name, newPos);
            m_StepsDoneInOneDirection++;
        }

        private void ChangeDirection() {
            m_StepsDoneInOneDirection = 0;
            m_MoveDirection *= -1;
        }
    }
}
