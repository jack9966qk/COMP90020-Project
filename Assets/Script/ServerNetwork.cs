using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.Networking;

public class ServerNetwork {
	public static GlobalState GlobalState;
	public static StateChange StateChange = null;
	static int numPlayers = 0;
	static HashSet<int> submitted = new HashSet<int>();
	static Dictionary<int, int> connToPlayerId = new Dictionary<int, int>();
	public static void Start(int serverPort, GlobalState initialState) {
		GlobalState = initialState;
		
		// system messages
		NetworkServer.RegisterHandler(MsgType.Connect, OnServerConnect);
		NetworkServer.RegisterHandler(MsgType.Disconnect, OnServerDisconnect);
		NetworkServer.RegisterHandler(MsgType.Error, OnServerError);

		// application messages
		NetworkServer.RegisterHandler(NetworkMsgType.StateChangeSubmission, OnStateChangeSubmission);

		// start listening
		NetworkServer.Listen(serverPort);

		Debug.Log("Server listening for connections");
	}

	static void startGame() {
		// start the game by broadcasting global state for the first time
		Debug.Log("Start game");
		NetworkServer.SendToAll(NetworkMsgType.NewGlobalState, GlobalState);
	}

	static void OnStateChangeSubmission(NetworkMessage msg) {
		var change = msg.ReadMessage<StateChange>();
		if (StateChange == null) {
			StateChange = change;
		} else {
			StateChange.merge(change);
		}

		// check if all clients submitted
		if (submitted.Count >= numPlayers) {
			submitted.Clear();
			StateChange = null;
			NetworkServer.SendToAll(NetworkMsgType.NewGlobalState, GlobalState);
		}
	}

	static void OnServerConnect(NetworkMessage msg) {
		Debug.Log("Connected");
		// associate connected client to player ID
		connToPlayerId[msg.conn.connectionId] = numPlayers;
		numPlayers += 1;

		// start the game if all players connected
		startGame();
	}

	static void OnServerDisconnect(NetworkMessage msg) {
		Debug.Log("Disconnected");
	}

	static void OnServerError(NetworkMessage msg) {
		Debug.Log("Error");
	}
}
