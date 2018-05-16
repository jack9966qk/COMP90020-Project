using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class ServerNetwork : MonoBehaviour {
	Dictionary<int, StateChange> stateChanges
		= new Dictionary<int, StateChange>();
	int numPlayers = 0;
	HashSet<int> submitted = new HashSet<int>();
	Dictionary<int, int> connToPlayerId = new Dictionary<int, int>();
	Dictionary<int, int> clientTimes = new Dictionary<int, int>();
	int serverTime = 0;

	public ServerLogic ServerLogic;
	public void StartServer(int serverPort) {
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

	void startGame() {
		// start the game by broadcasting global state for the first time
		Debug.Log("Start game");
		ServerLogic.Initialise(numPlayers);

		// for each client, send player id
		foreach (var connId in connToPlayerId.Keys) {
			var msg = new IntegerMessage();
			msg.value = connToPlayerId[connId];
			NetworkServer.SendToClient(connId, NetworkMsgType.AssignPlayerId, msg);
		}
		
		broadcastGlobalState();
	}

	void broadcastGlobalState() {
		serverTime += 1;
		StartCoroutine(sendGlobalState());		
	}

	IEnumerator sendGlobalState() {
		yield return new WaitForSeconds(Constants.ArtificialLatency);
        foreach (KeyValuePair<int, LocalState> playerState in ServerLogic.GlobalState.LocalStates) {
            Debug.Log("stationary? network= " + playerState.Value.PlayerState.Stationary);
        }
        foreach (var connId in connToPlayerId.Keys) {
			var playerId = connToPlayerId[connId];
			NetworkServer.SendToClient(
				connId,
				NetworkMsgType.NewGlobalState,
				new GlobalStateMessage {
					LogicTime = new Vector2(clientTimes[playerId], serverTime),
					GlobalState = ServerLogic.GlobalState
				}
			);
		}
	}

	void OnStateChangeSubmission(NetworkMessage msg) {
		Debug.Log("state change received");
		var stateChangeMsg = msg.ReadMessage<StateChangeMessage>();
		var change = stateChangeMsg.StateChange;
		var connId = msg.conn.connectionId;
		var playerId = connToPlayerId[connId];
		if (!stateChanges.ContainsKey(playerId)) {
			stateChanges[playerId] = change;
		} else {
			stateChanges[playerId].merge(change);
		}
		var time = stateChangeMsg.LogicTime;
		clientTimes[playerId] = (int)time.x;

		submitted.Add(connToPlayerId[msg.conn.connectionId]);

		// check if all clients submitteds
        // TODO stretch goal - fix this ---------------
		if (submitted.Count >= numPlayers) {
			submitted.Clear();
			// Call serverlogic to update global game state
			ServerLogic.ApplyStateChange(stateChanges);
			stateChanges.Clear();
			Debug.Log("Send new global state");
			broadcastGlobalState();
		}
	}

	void OnClientConnect(NetworkMessage msg) {
		Debug.Log("Connected");
		// associate connected client to player ID
		connToPlayerId[msg.conn.connectionId] = numPlayers;
		// initialise client time
		clientTimes[numPlayers] = 0;
		numPlayers += 1;

		// start the game if all players connected
		if (numPlayers >= 2) {
			startGame();
		}
	}

	void OnClientDisconnect(NetworkMessage msg) {
		Debug.Log("Disconnected");
	}

	void OnClientError(NetworkMessage msg) {
		Debug.Log("Error");
	}
}
