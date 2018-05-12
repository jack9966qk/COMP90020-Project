﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GlobalState : MessageBase {
	// local state for each player
	Dictionary<int, LocalState> LocalStates
		= new Dictionary<int, LocalState>();

	// bullet states shared globally
	Dictionary<int, BulletState> BulletStates
		= new Dictionary<int, BulletState>();

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

	public static GlobalState Initialise(int numPlayers) {
		var localStates = new Dictionary<int, LocalState>();
		for (var i = 0; i < numPlayers; i++) {
			localStates[i] = new LocalState {
				PlayerState = new PlayerState {
					Position = new Vector2(0, 0),
					Orientation = Direction.Up,
					HP = 100f
				}
			};
		}
		return new GlobalState {
			LocalStates = localStates,
			BulletStates = new Dictionary<int, BulletState>()
		};
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

		var bullets = new Dictionary<int, BulletState>();
		var numBullet = reader.ReadInt32();
		for (var i = 0; i < numBullet; i++) {
			var id = reader.ReadInt32();
			var state = new BulletState();
			state.Deserialize(reader);
			bullets[id] = state;
		}

		LocalStates = locals;
		BulletStates = bullets;
	}
}
