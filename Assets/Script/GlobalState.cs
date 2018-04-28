using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalState {
	Dictionary<int, LocalState> LocalStates
		= new Dictionary<int, LocalState>();

	public LocalState GetLocalState(int playerID) {
		return LocalStates[playerID];
	}

	public void PutLocalState(int playerID, LocalState state) {
		LocalStates[playerID] = state;
	}
}
