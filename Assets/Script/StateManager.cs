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
    private Queue<BufferItem> updateHistory = new Queue<BufferItem>();
	GlobalState GlobalState = null;
    public GameController GameController;
    private static int? PID = null;
    private float prevTime = 0;

    private static int bulletIdCounter = 0;

	public GlobalState GetApproxState() {
        float currentTime = Time.time;
        //update all bullets
        if (GlobalState != null) {
            GlobalState currentState = GlobalState;
            foreach (KeyValuePair<string, BulletState> bullet in currentState.BulletStates) {
                float distance = (currentTime-prevTime) * Constants.BulletSpeed;
                float x = bullet.Value.Position.x;
                float y = bullet.Value.Position.y;
                //Debug.Log("before:" + transform.position);
                switch (bullet.Value.Direction) {
                    case Direction.Up:
                        this.transform.position = new Vector2(x, y + distance);
                        break;
                    case Direction.Down:
                        this.transform.position = new Vector2(x, y - distance);
                        break;
                    case Direction.Left:
                        this.transform.position = new Vector2(x - distance, y);
                        break;
                    case Direction.Right:
                        this.transform.position = new Vector2(x + distance, y);
                        break;
                    default:
                        break;
                }
                //Debug.Log("after:" + transform.position);
                bullet.Value.Position = transform.position;
            }
            GlobalState.DebugBulletStates();
        }
        prevTime = currentTime;
		return GlobalState;
	}

    // call when local player moves
        
	public void Move(Vector2 pos, Direction orientation) {
        SetPID();
        StateChange update = new StateChange {
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
        //Debug.Log("State manager receive bullet");
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
        //var existingBullets = GlobalState.BulletStates;
        //var newBullets = serverState.BulletStates;
        //remove old items from updateHistory 
        while (updateHistory.Count > 0 && updateHistory.Peek().TimeStamp.x < logictime.x) {
            updateHistory.Dequeue();
        }

        //rebuild State
        Debug.Log("future predictions" + updateHistory.Count);
        foreach (BufferItem update in updateHistory) {
            // Debug.Log(update.TimeStamp);
            serverState.ApplyStateChange(PID.Value, update.Update);
        } if (GlobalState == null) {
            var playerIds = serverState.LocalStates.Keys;
            GameController.Initialise(playerIds);
        }
        //Snapshot Interpolation goes here ------------------------------
        GlobalState = serverState;
    }

    public void SetPID() {
        if (PID == null) {
            PID = ClientNetwork.getPID();
        }
    }

	// Use this for initialization
	void Start () {
        ClientNetwork.StateManager = this;
	}
}
