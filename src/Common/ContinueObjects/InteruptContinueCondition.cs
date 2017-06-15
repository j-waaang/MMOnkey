namespace JYW.ThesisMMO.Common.ContinueObjects {
    public class InteruptContinueCondition : ActionContinueCondition {
        
        public override void Start() {
        }

        private void OnInterupt() {
            RaiseContinueEvent(ContinueReason.Interupted);
        }
    }
}
