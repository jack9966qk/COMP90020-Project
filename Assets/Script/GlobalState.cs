using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GlobalState {
	// local state for each player
	Dictionary<int, LocalState> LocalStates
		= new Dictionary<int, LocalState>();

	// bullet states shared globally
	Dictionary<int, BulletState> BulletStates;

	public BulletState GetBulletState(int bulletID) {
		return BulletStates[bulletID];
	}

	public void PutBulletState(int bulletID, BulletState state) {
		BulletStates[bulletID] = state;
	}

	public LocalState GetLocalState(int playerID) {
		return LocalStates[playerID];
	}

	public void PutLocalState(int playerID, LocalState state) {
		LocalStates[playerID] = state;
	}

	public void WriteToBuffer(NetworkWriter writer) {
		writer.Write(LocalStates.Count);
		foreach (var key in LocalStates.Keys) {
			writer.Write(key);
			LocalStates[key].WriteToBuffer(writer);
		}
		writer.Write(BulletStates.Count);
		foreach (var key in BulletStates.Keys) {
			writer.Write(key);
			BulletStates[key].WriteToBuffer(writer);
		}
	}

	static public GlobalState ReadFromBuffer(NetworkReader reader) {
		var locals = new Dictionary<int, LocalState>();
		var numLocal = reader.ReadInt32();
		for (var i = 0; i < numLocal; i++) {
			var id = reader.ReadInt32();
			var state = LocalState.ReadFromBuffer(reader);
			locals[id] = state;
		}

		var bullets = new Dictionary<int, BulletState>();
		var numBullet = reader.ReadInt32();
		for (var i = 0; i < numLocal; i++) {
			var id = reader.ReadInt32();
			var state = BulletState.ReadFromBuffer(reader);
			bullets[id] = state;
		}

		return new GlobalState {
			LocalStates = locals,
			BulletStates = bullets
		};
	}
}
