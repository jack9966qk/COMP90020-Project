using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour {
	GlobalState GlobalState
		= new GlobalState();

	public GlobalState GetApproxState() {
		return GlobalState;
	}

	public void SetLocalState(int playerID, LocalState state) {
		GlobalState.PutLocalState(playerID, state);
	}

	public void InitialiseLocalState(int playerID) {
		GlobalState.PutLocalState(playerID, new LocalState());
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
