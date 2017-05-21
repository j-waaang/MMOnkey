namespace JYW.ThesisMMO.UnityClient.CharacterSelection {

    using UnityEngine;
    using UnityEngine.UI;

    public class WeaponSelectionController : MonoBehaviour {

        [SerializeField] private Dropdown m_WeaponDropDown;

        internal int GetSelectedWeapon() {
            return m_WeaponDropDown.value;
        }
    }
}