using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.Networking;

public class ClientNetwork {
	public static GlobalState serverState;
	public static StateChange changeToSend = null;
	static NetworkClient client = null;

	public static void Start(string serverIp, int serverPort) {
		client = new NetworkClient();

		// system messages
		client.RegisterHandler(MsgType.Connect, OnClientConnect);
		client.RegisterHandler(MsgType.Disconnect, OnClientDisconnect);
		client.RegisterHandler(MsgType.Error, OnClientError);

		// application messages
		client.RegisterHandler(NetworkMsgType.NewGlobalState, OnNewGlobalState);

		// connect
		client.Connect(serverIp, serverPort);

		Debug.Log("Connecting to server");
	}

	static void OnClientConnect(NetworkMessage msg) {
		Debug.Log("Connected");
	}

	static void OnClientDisconnect(NetworkMessage msg) {
		Debug.Log("Disconnected");
	}

	static void OnClientError(NetworkMessage msg) {
		Debug.Log("Error");
	}

	static void OnNewGlobalState(NetworkMessage msg) {
		// receive global state
		serverState = msg.ReadMessage<GlobalState>();
		Debug.Log("New global state received");

		if (changeToSend == null) {
			// TODO
		}
		// send state change
		client.Send(NetworkMsgType.StateChangeSubmission, changeToSend);
		// reset state change TODO
		changeToSend = null;
	}
}
