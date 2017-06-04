namespace JYW.ThesisMMO.UnityClient.Characters.Player {

    using UnityEngine;
    using JYW.ThesisMMO.Common.Entities;

    public class CharacterState : MonoBehaviour {

        internal ActionState ActionState { get; set; }
        internal MovementState MovementState { get; set; }
    }
}