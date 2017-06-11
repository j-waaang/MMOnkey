namespace JYW.ThesisMMO.MMOServer.ActionObjects {
    internal class InteruptContinueCondition : ActionContinueCondition {

        internal InteruptContinueCondition(ActionObject actionObject) :
            base(actionObject){

        }

        internal override void Start() {
        }

        private void OnInterupt() {
            RaiseContinueEvent(ContinueReason.Interupted);
        }
    }
}
