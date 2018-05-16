using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour {
    // struct for each item in the buffer
    struct BufferItem {
        public Vector2 TimeStamp;
        public StateChange Update;
    }
    // current logical time, with x = client time, y = server time
    Vector2 logictime = new Vector2(0, 0);
    // buffer to store recent update events
    private Queue<BufferItem> updateHistory = new Queue<BufferItem>();
	GlobalState GlobalState = null;
    public GameController GameController;
    private static int? PID = null;
    private static int bulletIdCounter = 0;
	public GlobalState GetApproxState() {
        return GlobalState;
	}

    void Update() {
        if (PID == null) PID = ClientNetwork.getPID();
        if (GlobalState == null) return;
        // dead reckoning implementation
        GlobalState currentState = GlobalState;
        // update bullet positions
        foreach (KeyValuePair<string, BulletState> bullet in currentState.BulletStates) {
            float distance = (Time.deltaTime) * Constants.BulletSpeed;
            bullet.Value.Position = DirectionUtil.move(
                bullet.Value.Position,
                bullet.Value.Direction,
                distance);
        }
        
        // update player positions
        // P=P0+(V0*Delta(t)) P is current position, P0 is last known position,
        // V0 is player Speed or if player isn't moving 0,
        // delta(t) is the change in physical time
        foreach (var pair in currentState.LocalStates) {
            if (PID == null) break;
            if (pair.Key == PID) continue;
            var playerState = pair.Value.PlayerState;
            if (playerState.Stationary != true) {
                // player movement
                float distance = Time.deltaTime * Constants.PlayerSpeed;
                playerState.Position = DirectionUtil.move(
                    playerState.Position,
                    playerState.Orientation,
                    distance);
            }
            GlobalState.LocalStates[pair.Key].PlayerState = playerState;   
        }  
    }

    // call when local player moves
	public void Move(Vector2 pos, Direction orientation) {
        SetPID();
        StateChange update = new StateChange {
            NewPosition = pos,
            NewOrientation = orientation
        };
        // update server state
        ApplyStateChange(update);
	}

	public void ShootBullet(BulletState bullet) {
        SetPID();
        bullet.BulletID = PID.Value.ToString() + "-" + bulletIdCounter.ToString();
        bulletIdCounter += 1;
        var bulletsCreated = new HashSet<BulletState>();
        bulletsCreated.Add(bullet);
        StateChange update = new StateChange {
            BulletsCreated = bulletsCreated
        };
        update.BulletsCreated.Add(bullet);
        
        // add bullet state to Global State
        ApplyStateChange(update);
    }

	public void ApplyStateChange(StateChange stateChange) {
        logictime.Set(logictime.x + 1, logictime.y);
        // apply local state change
        GlobalState.ApplyStateChange(PID.Value,stateChange);
        // store state in history buffer
        updateHistory.Enqueue(new BufferItem {
            TimeStamp = logictime,
            Update = stateChange
        });

        // send the state change to server and attach current timestamp
        ClientNetwork.UpdateStateChange(stateChange,logictime);
	}

	public void UpdateServerState(GlobalState serverState, Vector2 logictime) {
        // remove old items from updateHistory
        // all updates before last server state can be discarded
        while (updateHistory.Count > 0 && updateHistory.Peek().TimeStamp.x <= logictime.x) {
            updateHistory.Dequeue();
        }
        
        // rebuild state by applying all local changes since last server state
        foreach (BufferItem update in updateHistory) { 
            serverState.ApplyStateChange(PID.Value, update.Update);
        } if (GlobalState == null) {
            var playerIds = serverState.LocalStates.Keys;
            GameController.Initialise(playerIds);
        }
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
