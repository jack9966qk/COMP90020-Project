using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GlobalState : MessageBase {
	// local state for each player
	public Dictionary<int, LocalState> LocalStates
		= new Dictionary<int, LocalState>();

	// bullet states shared globally
	public Dictionary<string, BulletState> BulletStates
		= new Dictionary<string, BulletState>();

    public void ApplyStateChange(int playerId, StateChange stateChange) {
        LocalStates[playerId].PlayerState.Position = stateChange.NewPosition;
        foreach (var bulletState in stateChange.BulletsCreated) {
            BulletStates[bulletState.BulletID] = (bulletState);
        }
    }


	public override void Serialize(NetworkWriter writer) {
		writer.Write(LocalStates.Count);
		foreach (var key in LocalStates.Keys) {
			writer.Write(key);
			LocalStates[key].Serialize(writer);
		}
		writer.Write(BulletStates.Count);
		foreach (var key in BulletStates.Keys) {
			writer.Write(key);
			BulletStates[key].Serialize(writer);
		}
	}

	public override void Deserialize(NetworkReader reader) {
		var locals = new Dictionary<int, LocalState>();
		var numLocal = reader.ReadInt32();
		for (var i = 0; i < numLocal; i++) {
			var id = reader.ReadInt32();
			var state = new LocalState();
			state.Deserialize(reader);
			locals[id] = state;
		}

		var bullets = new Dictionary<string, BulletState>();
		var numBullet = reader.ReadInt32();
		for (var i = 0; i < numBullet; i++) {
			var id = reader.ReadString();
			var state = new BulletState();
			state.Deserialize(reader);
			bullets[id] = state;
		}

		LocalStates = locals;
		BulletStates = bullets;
	}
}
