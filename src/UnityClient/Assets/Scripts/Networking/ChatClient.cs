using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using System.Threading;
using UnityEngine;

public class ChatClient : IPhotonPeerListener {
    private bool connected;
    PhotonPeer peer;
    Thread thread;
    private static string ip = "188.174.190.233:4530";
    //private static string ip = "172.16.80.1:4530";
    //private static string ip = "127.0.0.1:4530";

    private static ChatClient client;

    public static void Start() {
        client = new ChatClient();
        client.peer = new PhotonPeer(client, ConnectionProtocol.Tcp);
        // connect
        client.DebugReturn(DebugLevel.INFO, "Connecting to server at " + ip + " using TCP");
        client.peer.Connect(ip, "ChatServer");
        // client needs a background thread to dispatch incoming messages and send outgoing messages
        client.Run();
        //while (true) {
        //    if (!client.connected) { continue; }
        //    // read input
        //    string buffer = Console.ReadLine();
        //    // send to server
        //    var parameters = new Dictionary<byte, object> { { 1, buffer } };
        //    client.peer.OpCustom(1, parameters, true);
        //}
    }

    public static void Stop() {
        client.thread.Abort();
        client.peer.Disconnect();
    }

    private void UpdateLoop() {
        while (true) {
            peer.Service();
        }
    }

    public void Run() {
        thread = new Thread(UpdateLoop);
        thread.IsBackground = true;
        thread.Start();
    }

    #region IPhotonPeerListener

    public void DebugReturn(DebugLevel level, string message) {
        Debug.Log(string.Format("{0}: {1}", level, message));
    }

    public void OnEvent(EventData eventData) {
        DebugReturn(DebugLevel.INFO, eventData.ToStringFull());
        if (eventData.Code == 1) {
            DebugReturn(DebugLevel.INFO, string.Format("Chat Message: {0}", eventData.Parameters[1]));
        }
    }

    public void OnMessage(object messages) {
        throw new NotImplementedException();
    }

    public void OnOperationResponse(OperationResponse operationResponse) {
        DebugReturn(DebugLevel.INFO, operationResponse.ToStringFull());
    }

    public void OnStatusChanged(StatusCode statusCode) {
        switch (statusCode) {
            case StatusCode.Connect:
                DebugReturn(DebugLevel.INFO, "Connected");
                connected = true;
                break;
            default:
                DebugReturn(DebugLevel.ERROR, statusCode.ToString());
                break;
        }
    }

    #endregion
}