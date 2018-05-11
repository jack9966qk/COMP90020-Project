using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

public class LocalState {
    public PlayerState PlayerState;

    public void WriteToBuffer(NetworkWriter writer) {
		PlayerState.WriteToBuffer(writer);
	}

	static public LocalState ReadFromBuffer(NetworkReader reader) {
		return new LocalState {
            PlayerState = PlayerState.ReadFromBuffer(reader)
        };
	}
}