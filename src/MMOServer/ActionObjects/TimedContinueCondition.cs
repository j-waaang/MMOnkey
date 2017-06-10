namespace JYW.ThesisMMO.MMOServer.ActionObjects {
    using System;
    using System.Threading;

    /// <summary> 
    /// Time based ActionCondition.
    /// </summary>
    internal class TimedContinueCondition : ActionContinueCondition {

        private Thread m_Thread;

        internal TimedContinueCondition(TimeSpan waitTime) {
            // TODO: Also listen to interupt events;
            var m_Thread = new Thread(() => SleepAndContinue(waitTime));
        }

        internal void Start() {
            m_Thread.Start();
        }

        private void SleepAndContinue(TimeSpan sleepTime) {
            Thread.Sleep(sleepTime);
            RaiseContinueEvent(ContinueReason.ConditionFullfilled);
        }
    }
}
