namespace JYW.ThesisMMO.UnityClient {

    using CharacterSelection;
    using System;
    using UnityEngine;

    /// <summary>  
    ///  Holds game data & events relevant for main game logic.
    /// </summary> 
    internal static class GameData {

        internal static event Action<GameObject> TargetChangedEvent;
        internal static event Action<Vector2> AvatarPositionChangedEvent;

        internal static CharacterSetting characterSetting { get; set; }

        private static GameObject m_Target;
        internal static GameObject Target {
            get {
                return m_Target;
            }

            set {
                var oldValue = m_Target;
                m_Target = value;
                if (oldValue != value) {
                    TargetChangedEvent(value);
                }
            }
        }

        private static Vector2 clientCharacterPosition;
        internal static Vector2 ClientCharacterPosition {
            get {
                return clientCharacterPosition;
            }
            set {
                var oldValue = clientCharacterPosition;
                clientCharacterPosition = value;
                if (oldValue != value) {
                    AvatarPositionChangedEvent(value);
                }
            }
        }

        internal const float InterestDistance = 17.0f;
    }
}
