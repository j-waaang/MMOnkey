using JYW.ThesisMMO.Common.Codes;
using UnityEngine;

public class AOEHintCreator : MonoBehaviour {

    public void PlayDelayedAttackShape(ActionCode action, float waitForSeconds) {
        StartCoroutine(ActionHelper.WaitAndDo(waitForSeconds, () => PlayAttackShape(action)));
    }

    public void PlayAttackShape(ActionCode action) {
        GameObject shapeInst = null;
        switch (action) {
            case ActionCode.AxeAutoAttack:
                shapeInst = Instantiate(Resources.Load<GameObject>("ConeAOE"), transform) as GameObject;
                shapeInst.transform.localScale = new Vector3(2f, 0.2f, 2f);
                break;
            case ActionCode.BowAutoAttack:
                shapeInst = Instantiate(Resources.Load<GameObject>("RectangleAOE"), transform) as GameObject;
                shapeInst.transform.localScale = new Vector3(1f, 0.2f, 4f);
                break;
            case ActionCode.DistractingShot:
                shapeInst = Instantiate(Resources.Load<GameObject>("RectangleAOE"), transform) as GameObject;
                shapeInst.transform.localScale = new Vector3(.5f, 0.2f, 4f);
                break;
            case ActionCode.HammerBash:
                shapeInst = Instantiate(Resources.Load<GameObject>("CylinderAOE"), transform) as GameObject;
                break;
        }
        shapeInst.transform.forward = transform.forward;
    }
}
