﻿using System.Collections;
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
	GlobalState GlobalState {
        get {
           // Debug.Log("get global state");
            return _globalState;
        } set {
          //  Debug.Log("set global state");
            _globalState = value;
        }
    }
    GlobalState _globalState = null;
    public GameController GameController;
    private static int? PID = null;
    private float prevTime = 0;

    private static int bulletIdCounter = 0;

	public GlobalState GetApproxState() {
        // Debug.Log("GetApproxState");
        //  Debug.Log(GlobalState.LocalStates.Count);
  
      /*  foreach (var pair in GlobalState.LocalStates) {
            if (pair.Key == PID) continue;
           // Debug.Log(GlobalState.LocalStates[pair.Key].PlayerState.Position);
        }*/
        return GlobalState;
	}

    void Update() {
        //update all bullets
       // Debug.Log("Update start");
        if(PID == null) PID = ClientNetwork.getPID();
        if (GlobalState != null) {
            GlobalState currentState = GlobalState;
            foreach (KeyValuePair<string, BulletState> bullet in currentState.BulletStates) {
                float distance = (Time.deltaTime) * Constants.BulletSpeed;
                float x = bullet.Value.Position.x;
                float y = bullet.Value.Position.y;
                switch (bullet.Value.Direction) {
                    case Direction.Up:
                        bullet.Value.Position = new Vector2(x, y + distance);
                        break;
                    case Direction.Down:
                        bullet.Value.Position = new Vector2(x, y - distance);
                        break;
                    case Direction.Left:
                        bullet.Value.Position = new Vector2(x - distance, y);
                        break;
                    case Direction.Right:
                        bullet.Value.Position = new Vector2(x + distance, y);
                        break;
                    default:
                        break;
                }
            }
            
            foreach (var pair in currentState.LocalStates) {
                if (PID == null) break;
                if (pair.Key == PID) continue;
                var playerState = pair.Value.PlayerState;
                // player movement
                float distance = Time.deltaTime * Constants.PlayerSpeed;
                float x = playerState.Position.x;
                float y = playerState.Position.y;
                switch (playerState.Orientation) {
                    case Direction.Up:
                        playerState.Position = new Vector2(x, y + distance);
                        break;
                    case Direction.Down:
                        playerState.Position = new Vector2(x, y - distance);
                        break;
                    case Direction.Left:
                        playerState.Position = new Vector2(x - distance, y);
                        break;
                    case Direction.Right:
                        playerState.Position = new Vector2(x + distance, y);
                        break;
                    default:
                        break;
                }
                GlobalState.LocalStates[pair.Key].PlayerState = playerState;
                //Debug.Log(GlobalState.LocalStates[pair.Key].PlayerState.Position);
            }
           // GlobalState.DebugBulletStates();
        }
       // Debug.Log("Update End");
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
        if(PID == null) {
            PID = ClientNetwork.getPID();
        }
        //use current position to calculate desired position for each player
       /* if (updateHistory.Count > 0) {
            BufferItem currentUpdate = updateHistory.Dequeue();
            // Vector2 oldPos = currentUpdate.Update.NewPosition.Value;
            // Vector2 currentPos = GlobalState.LocalStates[PID.Value].PlayerState.Position;
            // float distance = Mathf.Abs(currentPos.x - oldPos.x) + Mathf.Abs(currentPos.y - oldPos.y);
            /* foreach (KeyValuePair<int, LocalState> player in serverState.LocalStates) {
                 if (player.Key == PID) {
                     continue;
                 }
                 Direction orientation = player.Value.PlayerState.Orientation;
                 float x = player.Value.PlayerState.Position.x;
                 float y = player.Value.PlayerState.Position.y;
                 switch (orientation) {
                     case Direction.Up:
                         player.Value.PlayerState.Position = new Vector2(x, y + distance);
                         break;
                     case Direction.Down:
                         player.Value.PlayerState.Position = new Vector2(x, y - distance);
                         break;
                     case Direction.Left:
                         player.Value.PlayerState.Position = new Vector2(x - distance, y);
                         break;
                     case Direction.Right:
                         player.Value.PlayerState.Position = new Vector2(x + distance, y);
                         break;
                     default:
                         break;

                 }
             }
        }*/
        //rebuild State
        //Debug.Log("future predictions" + updateHistory.Count);
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
