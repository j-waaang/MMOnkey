using JYW.ThesisMMO.Common.Codes;
using JYW.ThesisMMO.UnityClient.Core.MessageHandling.Events;
using System;
using UnityEngine;

public class SkillEntitySpawner : MonoBehaviour{

    private void Awake() {
        EventOperations.NewSkillEntityEvent += CreateSkillEntity;
    }

    public static void CreateSkillEntity(string name, ActionCode skill, Vector3 position) {
        Debug.Log("Creating remote skill entity");
        var prefabName = Enum.GetName(typeof(ActionCode), skill);
        prefabName += "Entity";
        Debug.LogFormat("Try instantiating object with name {0}", name);
        var go = Instantiate(Resources.Load(prefabName, typeof(GameObject)), position, Quaternion.identity) as GameObject;
        go.name = name;
    }

    public static void CreateSkillEntity(ActionCode skill, Vector3 position) {
        var prefabName = Enum.GetName(typeof(ActionCode), skill);
        prefabName += "Entity";
        Debug.LogFormat("Try instantiating object with name {0}", prefabName);
        Instantiate(Resources.Load(prefabName, typeof(GameObject)), position, Quaternion.identity);
    }
}
