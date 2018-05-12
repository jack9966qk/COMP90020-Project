using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.Networking;

public class ServerNetwork : MonoBehaviour {
	public static GlobalState GlobalState;
	public static StateChange StateChange = null;
	static int numPlayers = 0;
	static HashSet<int> submitted = new HashSet<int>();
	static Dictionary<int, int> connToPlayerId = new Dictionary<int, int>();
	public static void StartServer(int serverPort) {
		// GlobalState = initialState;
		
		// system messages
		NetworkServer.RegisterHandler(MsgType.Connect, OnClientConnect);
		NetworkServer.RegisterHandler(MsgType.Disconnect, OnClientDisconnect);
		NetworkServer.RegisterHandler(MsgType.Error, OnClientError);

		// application messages
		NetworkServer.RegisterHandler(NetworkMsgType.StateChangeSubmission, OnStateChangeSubmission);

		// start listening
		NetworkServer.Listen(serverPort);

		Debug.Log("Server listening for connections");
	}

	public void Start() {
		StartServer(Constants.Port);
	}

	static void startGame() {
		// start the game by broadcasting global state for the first time
		Debug.Log("Start game");
		GlobalState = GlobalState.Initialise(numPlayers);
		NetworkServer.SendToAll(NetworkMsgType.NewGlobalState, GlobalState);
	}

	static void OnStateChangeSubmission(NetworkMessage msg) {
		var change = msg.ReadMessage<StateChange>();
		if (StateChange == null) {
			StateChange = change;
		} else {
			StateChange.merge(change);
		}

		// check if all clients submitteds
        // stretch goal - fix this ------------------------------------------------------------------
		if (submitted.Count >= numPlayers) {
			submitted.Clear();
			StateChange = null;
			Debug.Log("Send new global state");
			NetworkServer.SendToAll(NetworkMsgType.NewGlobalState, GlobalState);
		}
	}

	static void OnClientConnect(NetworkMessage msg) {
		Debug.Log("Connected");
		// associate connected client to player ID
		connToPlayerId[msg.conn.connectionId] = numPlayers;
		numPlayers += 1;

		// start the game if all players connected
		if (numPlayers >= 1) {
			startGame();
		}
	}

	static void OnClientDisconnect(NetworkMessage msg) {
		Debug.Log("Disconnected");
	}

	static void OnClientError(NetworkMessage msg) {
		Debug.Log("Error");
	}
}
