using System;
using System.IO;
using UnityEngine;
using JYW.ThesisMMO.UnityClient.Core.MessageHandling.Events;
using JYW.ThesisMMO.UnityClient.Core.MessageHandling.Requests;
using System.Globalization;
using JYW.ThesisMMO.UnityClient;

public class EvaluationManager : MonoBehaviour {

    //UPS = UpdatesPerSecond
    public delegate void NewPosUPS(int pos, int filPos);
    public static event NewPosUPS UPSevent;

    private const string ToggleAiKey = "ToggleAi";
    private const string LogFileName = "Log.text";

    private int m_TmpMoveCounter = 0;
    private int m_TmpFiltMoveCounter = 0;
    private int m_TotalMoveCounter = 0;
    private int m_TotalFiltMoveCounter = 0;
    private float m_LastUpdateTime = 0F;
    private int m_LogCounter = 10;
    private bool m_Logging = false;
    private bool m_Dynmode = false;

    private StreamWriter m_CurrentWriter;

    private void Awake() {
        EventOperations.MoveEvent += OnMove;
        EventOperations.FilteredMoveEvent += OnFilteredMove;
    }

    private void StartLogging() {
        CreateLogFile();
        ResetCounters();
        m_Logging = true;
        m_LastUpdateTime = Time.time;
    }

    private void CreateLogFile() {
        var curFilename = DateTime.Now.ToString(new CultureInfo("de-DE")) + " " + LogFileName;
        curFilename = curFilename.Replace(':', '-');
        if (File.Exists(curFilename)) {
            Debug.LogError(curFilename + " already exists. Appending on existing file.");
            m_CurrentWriter = File.AppendText(curFilename);
        }
        else {
            m_CurrentWriter = File.CreateText(curFilename);
        }
        m_CurrentWriter.WriteLine("Starting new log.");
        Debug.Log("Starting new Log");
        var character = GameData.characterSetting.GetWeaponAndSkills();
        m_CurrentWriter.WriteLine("Character: {0}", character);
    }

    private void Update () {
        if (Input.GetButtonDown(ToggleAiKey)) {
            RequestOperations.ToggleAiLoopRequest();
            StartLogging();
        }

        if (Input.GetKeyDown(KeyCode.L)) {
            m_Dynmode = !m_Dynmode;
        }

        if(Time.time - m_LastUpdateTime >= 1F) {
            m_LastUpdateTime = Time.time;
            UPSevent(m_TmpMoveCounter, m_TmpFiltMoveCounter);
            if (m_Logging) {
                LogCounters();
            }
            ResetCounters();
        }
	}

    private void LogCounters() {
        m_CurrentWriter.WriteLine("Move/s: {0}, FilteredMove/s: {1}, PercentFiltered: {2}",
            m_TmpMoveCounter,
            m_TmpFiltMoveCounter,
            (float)m_TmpFiltMoveCounter / (float)(m_TmpFiltMoveCounter + m_TmpMoveCounter)
            );
        m_LogCounter--;
        if (m_Dynmode && m_LogCounter == 5) {
            RequestOperations.DistractingShotRequest(new Vector3(1F, 0, 1F));
        }
        if (m_LogCounter == 0) {
            FinishLogging();
        }
    }

    private void FinishLogging() {
        m_CurrentWriter.WriteLine(
            "Finished eval. MoveCounter: {0}, FilterCounter: {1}, Percent Filtered: {2}%",
            m_TotalMoveCounter,
            m_TotalFiltMoveCounter,
            (float)m_TotalFiltMoveCounter / (float)(m_TotalFiltMoveCounter + m_TotalMoveCounter)
            );
        m_CurrentWriter.Close();
        m_Logging = false;
        Debug.Log("Finished Logging");
    }

    private void ResetTotalCounters() {
        m_TotalFiltMoveCounter = 0;
        m_TotalMoveCounter = 0;
    }

    private void ResetCounters() {
        m_TmpMoveCounter = 0;
        m_TmpFiltMoveCounter = 0;
    }

    private void OnFilteredMove(string name, Vector3 pos) {
        m_TmpFiltMoveCounter++;
        m_TotalFiltMoveCounter++;
    }

    private void OnMove(string name, Vector3 pos) {
        m_TmpMoveCounter++;
        m_TotalMoveCounter++;
    }
}
