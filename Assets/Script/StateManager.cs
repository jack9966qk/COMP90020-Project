using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour {
	GlobalState GlobalState
		= new GlobalState();
    Vector2 logictime = new Vector2(0, 0); //TO-DO
    public GameObject PlayerPrefab;
    public GameObject BulletPrefab;
    private static int? PID = null;
	public GlobalState GetApproxState() {
		return GlobalState;
	}

    // call when local player moves
	public void Move(Vector2 pos) {
        SetPID();
        StateChange update = new StateChange();
        update.NewPosition = pos;
        //update local state
        var playerState = GlobalState.LocalStates[PID.Value].PlayerState;
        playerState.Position = pos;
        //update server state
        ApplyStateChange(update);
	}

	public void ShootBullet(BulletState bullet) {
        SetPID();
        StateChange update = new StateChange();
        update.BulletsCreated.Add(bullet);
        //add bullet state to Global State
        ApplyStateChange(update);
        
    }

	public void ApplyStateChange(StateChange stateChange) {
        //send the state change to server
        logictime.Set(logictime.x + 1, logictime.y);
        ClientNetwork.UpdateStateChange(stateChange,logictime);
	}

	public void UpdateServerState(GlobalState serverState, Vector2 logictime) {
        // TODO..
        var existingBullets = GlobalState.BulletStates;
        var newBullets = serverState.BulletStates;
        // bullets created
        //foreach (var bulletId in newBullets.Keys) {
        //    if (!existingBullets.ContainsKey(bulletId) {
        //        // create new bullet
        //        var bulletObject = Instantiate(BulletPrefab, new Vector3(), new Quaternion());
        //        bulletObject.GetComponent<Bullet>().State = newBullets[bulletId];
        //    }
        //}
        GlobalState = serverState;
	}

    public void SetPID() {
        if(PID == null) {
            PID = ClientNetwork.getPID();
        }
    }

	// Use this for initialization
	void Start () {
        ClientNetwork.StateManager = this;
	}
	
	// Update is called once per frame
	void Update () {
		// do approximation
		// also apply transition (smoothing)
		// update gloabl objects (bullets)
	}
}
