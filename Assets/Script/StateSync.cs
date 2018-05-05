using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class StateSync {
	public static Thread Thread = new Thread(mainLoop);
	static GlobalState serverState;

	public static GlobalState GetServerState() {
		lock (serverState) {
			return serverState;
		}
	}

	public static void SetLocalState(int playerId, LocalState s) {
		lock (serverState) {
			serverState.PutLocalState(playerId, s);
		}
	}

	static void mainLoop() {
		while (true) {
			// receive state from server
			// ...
			
			// send local state to server
			// ...
		}
	}
}
