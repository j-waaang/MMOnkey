using JYW.ThesisMMO.UnityClient;
using JYW.ThesisMMO.UnityClient.Util;
using UnityEngine;

public class TargetSelection : MonobehaviourSingleton<TargetSelection> {

    protected TargetSelection() { }

    private void Update() {
        if (Input.GetButtonDown("AutoAttack") || Input.GetButtonDown("Select")) {
            SetTarget();
        }
    }

    public bool SetTarget() {
        var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(mouseRay, out hit) && hit.collider.tag == "Enemy") {
            GameData.Target = hit.collider.gameObject;
            return true;
        }
        else {
            GameData.Target = null;
            return false;
        }
    }
}
