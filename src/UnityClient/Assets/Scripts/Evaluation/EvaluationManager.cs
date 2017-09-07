using System;
using UnityEngine;
using JYW.ThesisMMO.UnityClient.Core.MessageHandling.Events;
using JYW.ThesisMMO.UnityClient.Core.MessageHandling.Requests;

public class EvaluationManager : MonoBehaviour {

    //UPS = UpdatesPerSecond
    public delegate void NewPosUPS(int pos, int filPos);
    public static event NewPosUPS UPSevent;

    private const string ToggleAiKey = "ToggleAi";

    private int TmpMoveCounter = 0;
    private int TmpFiltMoveCounter = 0;

    private float LastUpdateTime = 0F;

    private void Awake() {
        EventOperations.MoveEvent += OnMove;
        EventOperations.FilteredMoveEvent += OnFilteredMove;
    }

	private void Update () {
        if (Input.GetButtonDown(ToggleAiKey)) {
            RequestOperations.ToggleAiLoopRequest();
        }

        if(Time.time - LastUpdateTime >= 1F) {
            LastUpdateTime = Time.time;
            UPSevent(TmpMoveCounter, TmpFiltMoveCounter);
            ResetCounters();
        }
	}

    private void ResetCounters() {
        TmpMoveCounter = 0;
        TmpFiltMoveCounter = 0;
    }

    private void OnFilteredMove(string name, Vector3 pos) {
        TmpFiltMoveCounter++;
    }

    private void OnMove(string name, Vector3 pos) {
        TmpMoveCounter++;
    }
}
