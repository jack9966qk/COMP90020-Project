using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour {
	GlobalState GlobalState
		= new GlobalState();
    Vector2 logictime = new Vector2(0, 0); //TO-DO
	public GlobalState GetApproxState() {
		return GlobalState;
	}

    // call when local player moves
	public void Move(Vector2 pos) {
        StateChange update = new StateChange();
        update.NewPosition = pos;
        ApplyStateChange(update);
	}

	public void ShootBullet(BulletState bullet) {
        StateChange update = new StateChange();
        update.BulletsCreated.Add(bullet);
        ApplyStateChange(update);
    }

	public void ApplyStateChange(StateChange stateChange) {
		//apply the state change to local
        
        //send the state change to server
		// ClientNetwork.UpdateStateChange(stateChange);
	}

	public void UpdateServerState(GlobalState serverState, Vector2 logictime) {
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
