namespace JYW.ThesisMMO.MMOServer.CombatActions {

    using JYW.ThesisMMO.Common.Codes;

    /// <summary> 
    /// Creates ActionObjects.
    /// </summary>
    internal class ActionObjectFactory {

        internal ActionObject CreateActionObject(CharacterActionCode actionCode) {
            switch (actionCode) {
                case CharacterActionCode.AxeAutoAttack:
                    break;
                case CharacterActionCode.BowAutoAttack:
                    break;
                case CharacterActionCode.Move:
                    break;
                case CharacterActionCode.Dash:
                    break;
                case CharacterActionCode.DistractingShot:
                    break;
                case CharacterActionCode.FireStorm:
                    break;
                case CharacterActionCode.HammerBash:
                    break;
                case CharacterActionCode.OrisonOfHealing:
                    break;
                default:
                    break;
            }
            throw new System.NotImplementedException();
        }
    }
}
