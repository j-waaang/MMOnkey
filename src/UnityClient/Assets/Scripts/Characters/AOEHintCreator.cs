using JYW.ThesisMMO.Common.Codes;
using UnityEngine;

public class AOEHintCreator : MonoBehaviour {

    public void PlayDelayedAttackShape(ActionCode action) {
        StartCoroutine(ActionHelper.WaitAndDo(GetCastTime(action), () => PlayAttackShape(action)));
    }

    private static float GetCastTime(ActionCode action) {
        switch (action) {
            case ActionCode.AxeAutoAttack:
            case ActionCode.BowAutoAttack:
            case ActionCode.DistractingShot:
                return .5f;
            case ActionCode.FireStorm:
                return 2f;
            case ActionCode.HammerBash:
            case ActionCode.OrisonOfHealing:
            default:
                return 1f;
        }
    }

    public void PlayDelayedAttackShape(ActionCode action, float waitForSeconds) {
        StartCoroutine(ActionHelper.WaitAndDo(waitForSeconds, () => PlayAttackShape(action)));
    }

    public void PlayAttackShape(ActionCode action) {
        GameObject shapeInst = null;
        switch (action) {
            case ActionCode.AxeAutoAttack:
                shapeInst = Instantiate(Resources.Load<GameObject>("ConeAOE")) as GameObject;
                shapeInst.transform.localScale = new Vector3(2f, 0.2f, 2f);
                break;
            case ActionCode.BowAutoAttack:
                shapeInst = Instantiate(Resources.Load<GameObject>("RectangleAOE")) as GameObject;
                shapeInst.transform.localScale = new Vector3(1f, 0.2f, 4f);
                break;
            case ActionCode.DistractingShot:
                shapeInst = Instantiate(Resources.Load<GameObject>("RectangleAOE")) as GameObject;
                shapeInst.transform.localScale = new Vector3(.5f, 0.2f, 4f);
                break;
            case ActionCode.HammerBash:
                shapeInst = Instantiate(Resources.Load<GameObject>("CylinderAOE")) as GameObject;
                shapeInst.transform.localScale = new Vector3(5f, 0.2f, 5f);
                break;
        }
        if(shapeInst == null) { return; }
        shapeInst.transform.position = transform.position;
        shapeInst.transform.forward = transform.forward;
    }
}
