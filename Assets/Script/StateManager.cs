using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour {
    struct BufferItem
    {
        public Vector2 TimeStamp;
        public StateChange Update;
    }
    Vector2 logictime = new Vector2(0, 0);
    public GameObject PlayerPrefab;
    public GameObject BulletPrefab;
    private Queue<BufferItem> updateHistory;
	GlobalState GlobalState = null;
    public GameController GameController;
    private static int? PID = null;

    private static int bulletIdCounter = 0;

	public GlobalState GetApproxState() {
        //Debug.Log("GlobalState: " + GlobalState.LocalStates[(int)PID].PlayerState.Position);
		return GlobalState;
	}

    // call when local player moves
        
	public void Move(Vector2 pos, Direction orientation) {
        Debug.Log(pos.x + ";" + pos.y);
        SetPID();
        StateChange update = new StateChange
        {
            NewPosition = pos,
            NewOrientation = orientation
        };
        //update server state
        ApplyStateChange(update);
	}

	public void ShootBullet(BulletState bullet) {
        SetPID();
        bullet.BulletID = PID.Value.ToString() + "-" + bulletIdCounter.ToString();
        bulletIdCounter += 1;
        var bulletsCreated = new HashSet<BulletState>();
        bulletsCreated.Add(bullet);
        StateChange update = new StateChange
        {
            BulletsCreated = bulletsCreated
        };
        update.BulletsCreated.Add(bullet);
        Debug.Log("State manager receive bullet");
        //add bullet state to Global State
        ApplyStateChange(update);
    }

	public void ApplyStateChange(StateChange stateChange) {
        logictime.Set(logictime.x + 1, logictime.y);
        //apply local state change
        GlobalState.ApplyStateChange(PID.Value,stateChange);
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
        foreach (BufferItem update in updateHistory) {
            serverState.ApplyStateChange(PID.Value, update.Update);
        }
        if (GlobalState == null) {
            var playerIds = serverState.LocalStates.Keys;
            GameController.Initialise(playerIds);
        }
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
}
