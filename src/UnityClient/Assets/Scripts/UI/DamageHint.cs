using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class DamageHint : MonoBehaviour {

    private static readonly Color damageColor = Color.red;
    private static readonly Color healColor = Color.green;

    public void Initialize(int damage) {
        var textC = GetComponent<Text>();
        textC.text = damage.ToString();
        textC.color = damage < 0 ? damageColor : healColor;
    }
}
