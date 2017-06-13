using JYW.ThesisMMO.UnityClient;
using UnityEngine;

public class NameLoader : MonoBehaviour {

	private void Awake() {
        gameObject.name = GameData.characterSetting.Name;
    }
}
