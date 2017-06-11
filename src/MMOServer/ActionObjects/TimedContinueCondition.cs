namespace JYW.ThesisMMO.MMOServer.ActionObjects {
    using System;
    using System.Threading;

    /// <summary> 
    /// Time based ActionCondition.
    /// </summary>
    internal class TimedContinueCondition : ActionContinueCondition {

        private Thread m_Thread;

        internal TimedContinueCondition(ActionObject actionObject, TimeSpan waitTime)
            : base(actionObject) {
            m_Thread = new Thread(() => SleepAndContinue(waitTime));
        }

        internal override void Start() {
            m_Thread.Start();
        }

        private void SleepAndContinue(TimeSpan sleepTime) {
            Thread.Sleep(sleepTime);
            RaiseContinueEvent(ContinueReason.ConditionFullfilled);
        }
    }
}
