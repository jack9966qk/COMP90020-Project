using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class ClientNetwork : MonoBehaviour {
	static StateChange changeToSend = null;
	static NetworkClient client = null;
	static StateManager StateManager;
	static Vector2 clientLogicTime;

	static int? LocalPlayerId = null;

	public static void StartClient(string serverIp, int serverPort) {
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

	public void Start() {
		StartClient("127.0.0.1", Constants.Port);	
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

	public static void UpdateStateChange(StateChange stateChange, Vector2 logicTime) {
		changeToSend.merge(stateChange);
		clientLogicTime = logicTime;
	}

	static void OnPlayerIdAssignment(NetworkMessage msg) {
		Debug.Log("Player ID received");
		LocalPlayerId = msg.ReadMessage<IntegerMessage>().value;
	}

    public static int? getPID()
    {
        return LocalPlayerId;
    }

	static void OnNewGlobalState(NetworkMessage msg) {
		// receive global state
		var globalStateMsg = msg.ReadMessage<GlobalStateMessage>();
		StateManager.UpdateServerState(
			globalStateMsg.GlobalState,
			globalStateMsg.LogicTime);
		Debug.Log("New global state received");

        // no change
		if (changeToSend == null) {
			changeToSend = new StateChange();
		}
		// send state change
		client.Send(
			NetworkMsgType.StateChangeSubmission,
			new StateChangeMessage {
				StateChange = changeToSend,
				LogicTime = clientLogicTime
			}
		);
		// reset state change
		changeToSend = null;
	}
}
