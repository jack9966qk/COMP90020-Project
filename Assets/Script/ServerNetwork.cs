using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class ServerNetwork : MonoBehaviour {
	public static GlobalState GlobalState;
	static Dictionary<int, StateChange> stateChanges
		= new Dictionary<int, StateChange>();
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
		NetworkServer.RegisterHandler(
			NetworkMsgType.StateChangeSubmission,
			OnStateChangeSubmission);

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

		// for each client, send player id
		foreach (var connId in connToPlayerId.Keys) {
			var msg = new IntegerMessage();
			msg.value = connToPlayerId[connId];
			NetworkServer.SendToClient(connId, NetworkMsgType.AssignPlayerId, msg);
		}
		
		// broadcast global states
		NetworkServer.SendToAll(NetworkMsgType.NewGlobalState, GlobalState);
	}

	static void OnStateChangeSubmission(NetworkMessage msg) {
		Debug.Log("state change received");
		var change = msg.ReadMessage<StateChange>();
		var connId = msg.conn.connectionId;
		if (!stateChanges.ContainsKey(connId)) {
			stateChanges[connId] = change;
		} else {
			stateChanges[connId].merge(change);
		}

		submitted.Add(connToPlayerId[msg.conn.connectionId]);

		// check if all clients submitteds
        // TODO stretch goal - fix this ---------------
		if (submitted.Count >= numPlayers) {
			submitted.Clear();
			stateChanges.Clear();
			// TODO Call serverlogic to update global game state
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
		if (numPlayers >= 2) {
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
