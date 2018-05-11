using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.Networking;

public class ClientNetwork {
	public static Thread Thread = new Thread(mainLoop);
	static GlobalState serverState;
	static StateChange changeToSend;
	static int? channelId = null;
	static int? hostId = null;
	static int? connectionId = null;
	static int bufferSize = 1024;
	static byte[] readBuffer = new byte[bufferSize];
	static byte[] writeBuffer = new byte[bufferSize];

	public static GlobalState GetServerState() {
		lock (serverState) {
			return serverState;
		}
	}

	public static void ApplyStateChange(StateChange change) {
		//
	}

	public static void SetLocalState(int playerId, LocalState s) {
		lock (serverState) {
			serverState.PutLocalState(playerId, s);
		}
	}

	static void Start() {
		// init network layer
		NetworkTransport.Init();

		// configure network topology
		ConnectionConfig config = new ConnectionConfig();
		channelId = config.AddChannel(QosType.Reliable);
		int numConnection = 1;
		HostTopology topology = new HostTopology(config, numConnection);

		// create host
		hostId = NetworkTransport.AddHost(topology, 8888);

		// connect to server
		byte error;
		connectionId = NetworkTransport.Connect(hostId.Value, "127.0.0.1", 8888, 0, out error);

		NetworkEventType evnt = ReceiveFromServer();
		switch (evnt)
        {
            case NetworkEventType.ConnectEvent:
				Debug.Log("Connected");
				Thread.Start();
                break;
            default:
				Debug.Log("unexpected event type");
				Debug.Log(evnt);
				break;
        }
	}

	static NetworkEventType ReceiveFromServer() {
		int outConnectionId;
		int outChannelId;
		int receivedSize;
		byte error;
		NetworkEventType evnt = NetworkTransport.ReceiveFromHost(
			hostId.Value, out outConnectionId, out outChannelId,
			readBuffer, bufferSize, out receivedSize, out error);
		if (outConnectionId != connectionId) {
			Debug.Log("connection id mismatch");
		}
		if ((NetworkError)error != NetworkError.Ok) {
			Debug.Log(error);
		}
		return evnt;
	}

	static void mainLoop() {
		while (true) {
			// receive state from server
			var evnt = ReceiveFromServer();
			switch (evnt)
			{
				case NetworkEventType.DataEvent:
					var reader = new NetworkReader(readBuffer);
					serverState = GlobalState.ReadFromBuffer(reader);
					break;
				default:
					Debug.Log("unexpected event type");
					Debug.Log(evnt);
					break;
			}
			
			// send local state to server
			var writer = new NetworkWriter(writeBuffer);
			byte error;
			NetworkTransport.Send(
				hostId.Value, connectionId.Value, channelId.Value,
				writeBuffer, writer.Position, out error);
		}
	}
}
