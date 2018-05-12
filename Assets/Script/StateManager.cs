using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour {
	GlobalState GlobalState
		= new GlobalState();

	public GlobalState GetApproxState() {
		return GlobalState;
	}

    // call when local player moves
	public void Move(Vector2 pos) {
		//...
	}

    // call when a bullet is shooted by local player
	public void ShootBullet() {
		//...
	}

	public void ApplyStateChange(StateChange stateChange) {
		// TODO...
		ClientNetwork.UpdateStateChange(stateChange);
	}

	public void UpdateServerState(GlobalState serverState) {
		// TODO..
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		// do approximation
		// also apply transition (smoothing)
		// update gloabl objects (bullets)
	}
}
