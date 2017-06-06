using JYW.ThesisMMO.UnityClient;
using JYW.ThesisMMO.UnityClient.Util;
using UnityEngine;

public class TargetSelection : MonobehaviourSingleton<TargetSelection> {

    protected TargetSelection() { }

    void Update() {
        if (Input.GetButtonDown("AutoAttack") || Input.GetButtonDown("Select")) {
            var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            var target = Physics2D.Raycast(mouseRay.origin, mouseRay.direction);

            if (target.collider != null && target.collider.tag == "Enemy") {
                GameData.Target = target.collider.gameObject;
            }
            else {
                GameData.Target = null;
            }
        }
    }
}
