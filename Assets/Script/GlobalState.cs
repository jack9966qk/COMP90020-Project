using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
