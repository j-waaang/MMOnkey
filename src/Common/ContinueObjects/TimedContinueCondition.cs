namespace JYW.ThesisMMO.Common.ContinueObjects {
    using System;
    using System.Threading;

    /// <summary> 
    /// Time based ActionCondition.
    /// </summary>
    public class TimedContinueCondition : ActionContinueCondition {

        private Thread m_Thread;

        public TimedContinueCondition(TimeSpan waitTime) {
            m_Thread = new Thread(() => SleepAndContinue(waitTime));
        }

        public override void Start() {
            m_Thread.Start();
        }

        private void SleepAndContinue(TimeSpan sleepTime) {
            Thread.Sleep(sleepTime);
            RaiseContinueEvent(CallReason.ConditionFullfilled);
        }

        public override void Dispose() {
            m_Thread.Abort();
            base.Dispose();
        }
    }
}
