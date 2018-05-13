﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public BulletState State = new BulletState();
    public ServerLogic ServerLogic;
    int frame;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        
        if (ServerLogic != null) {
            var globalState = ServerLogic.GlobalState;
            if (!globalState.BulletStates.ContainsKey(State.BulletID)) {
                Debug.Log("Destroy");
                Destroy(this.gameObject);
            }
            // update position from state
            this.transform.position = globalState.BulletStates[State.BulletID].Position;
        } else {
            Move();
        }
        if (State == null) return;
    }

    public void Move()
    {
        float distance = Time.fixedDeltaTime * Constants.BulletSpeed;
        float x = this.State.Position.x;
        float y = this.State.Position.y;
        switch (State.Direction)
        {
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
        ServerLogic
            .GlobalState
            .BulletStates[State.BulletID]
            .Position = transform.position;
    }
}
