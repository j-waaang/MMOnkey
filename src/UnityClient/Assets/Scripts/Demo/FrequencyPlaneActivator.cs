using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrequencyPlaneActivator : MonoBehaviour {

    [SerializeField]
    private GameObject m_Plane;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.U)) {
            m_Plane.SetActive(!m_Plane.activeSelf);
        }
    }
}
