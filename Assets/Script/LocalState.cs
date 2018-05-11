using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

public class LocalState : MessageBase {
    public PlayerState PlayerState;

    public override void Serialize(NetworkWriter writer) {
		PlayerState.Serialize(writer);
	}

	public override void Deserialize(NetworkReader reader) {
		PlayerState = new PlayerState();
		PlayerState.Deserialize(reader);
	}
}