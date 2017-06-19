using System;
using System.Threading;

namespace JYW.ThesisMMO.Common.ContinueObjects {
    public class ContinueTick : ActionContinueCondition {

        private Thread m_Thread;
        private TimeSpan m_Interval;
        private int m_NumberOfTicks;

        public ContinueTick(TimeSpan interval, int numberOfTicks) {
            m_Interval = interval;
            m_NumberOfTicks = numberOfTicks;
            m_Thread = new Thread(() => SleepAndContinue(interval));
        }

        public override void Start() {
            m_Thread.Start();
        }

        private void SleepAndContinue(TimeSpan sleepTime) {
            while (m_NumberOfTicks > 1) {
                Thread.Sleep(sleepTime);
                RaiseContinueEvent(CallReason.Tick);
                m_NumberOfTicks--;
            }
            Thread.Sleep(sleepTime);
            RaiseContinueEvent(CallReason.LastTick);
        }
    }
}
