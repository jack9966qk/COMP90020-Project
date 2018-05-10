using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class ServerNetwork {
	public static Thread Thread = new Thread(mainLoop);

	static void mainLoop() {
		while (true) {
			// receive state changes from all clients

            // update global state (with game logic)

            // send back updated states
		}
	}
}
