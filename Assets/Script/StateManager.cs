using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour {
    struct BufferItem
    {
        public Vector2 TimeStamp;
        public StateChange Update;
    }
	GlobalState GlobalState
		= new GlobalState();
    Vector2 logictime = new Vector2(0, 0); //TO-DO
    public GameObject PlayerPrefab;
    public GameObject BulletPrefab;
    private Queue<BufferItem> updateHistory;
    private static int? PID = null;
    public int BulletCounter = 0;

	public GlobalState GetApproxState() {
		return GlobalState;
	}

    // call when local player moves
	public void Move(Vector2 pos) {
        SetPID();
        StateChange update = new StateChange();
        update.NewPosition = pos;
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
        logictime.Set(logictime.x + 1, logictime.y);
        //apply local state change
        GlobalState.ApplyStateChange(stateChange);
        //store state in history buffer
        updateHistory.Enqueue(new BufferItem {
            TimeStamp = logictime,
            Update = stateChange
        });
        //send the state change to server
        ClientNetwork.UpdateStateChange(stateChange,logictime);
	}

	public void UpdateServerState(GlobalState serverState, Vector2 logictime) {
        // TODO..
        var existingBullets = GlobalState.BulletStates;
        var newBullets = serverState.BulletStates;
        //remove old items from updateHistory 
        while (updateHistory.Peek().TimeStamp.x < logictime.x) {
            updateHistory.Dequeue();
        }
        //rebuild State
        foreach(BufferItem update in updateHistory) {
            serverState.ApplyStateChange(PID.Value,update.Update);
        }
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
