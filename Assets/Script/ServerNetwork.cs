using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.Networking;

public class ServerNetwork {
	public static GlobalState GlobalState;
	public static StateChange StateChange;

	static int numPlayers;
	static int numSubmitted;

	public static void Start(int serverPort) {
		// system messages
		NetworkServer.RegisterHandler(MsgType.Connect, OnServerConnect);
		NetworkServer.RegisterHandler(MsgType.Disconnect, OnServerDisconnect);
		NetworkServer.RegisterHandler(MsgType.Error, OnServerError);

		// application messages
		NetworkServer.RegisterHandler(NetworkMessageType.StateChangeSubmission, OnStateChangeSubmission);

		// start listening
		NetworkServer.Listen(serverPort);
	}

	public static void StartGame() {
		// start the game by broadcasting global state for the first time
		numPlayers = NetworkServer.connections.Count;
		NetworkServer.SendToAll(NetworkMessageType.NewGlobalState, GlobalState);
	}

	static void OnStateChangeSubmission(NetworkMessage msg) {
		var change = msg.ReadMessage<StateChange>();
		// StateChange.merge...

		// check if all clients submitted
		if (numSubmitted >= numPlayers) {
			NetworkServer.SendToAll(NetworkMessageType.NewGlobalState, GlobalState);
		}
	}

	static void OnServerConnect(NetworkMessage msg) {
		Debug.Log("Connected");
	}

	static void OnServerDisconnect(NetworkMessage msg) {
		Debug.Log("Disconnected");
	}

	static void OnServerError(NetworkMessage msg) {
		Debug.Log("Error");
	}
}
