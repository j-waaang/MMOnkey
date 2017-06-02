namespace JYW.ThesisMMO.UnityClient {

    using CharacterSelection;
    using System;
    using UnityEngine;

    /// <summary>  
    ///  Holds game data & events relevant for main game logic.
    /// </summary> 
    internal static class GameData {

        internal static Action<Vector2> ClientCharacterPositionChange;
        internal static CharacterSetting characterSetting { get; set; }

        public static Action<GameObject> TargetChangedAction;

        private static Vector2 clientCharacterPosition;

        internal static Vector2 ClientCharacterPosition {
            get {
                return clientCharacterPosition;
            }
            set {
                clientCharacterPosition = value;
                ClientCharacterPositionChange(value);
            }
        }

        internal const float InterestDistance = 17.0f;
    }
}
