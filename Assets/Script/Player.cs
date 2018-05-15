﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public PlayerState State;
    public StateManager StateManager;
    public GameController GameController;

    float lerpStartTime;
    float lerpTime = 0.1f;
    Vector2 lastPosition = new Vector2();
    Vector2 targetPosition = new Vector2();

	// Use this for initialization
	void Start () {
		
	}

    void updateTransform() {
        if (State == null) return;
        if (State.HP < Constants.PlayerHP && State.HP > (Constants.PlayerHP / 2))
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        else if (State.HP <= (Constants.PlayerHP / 2))
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
        var playerPos = State.Position;
        if (playerPos != targetPosition) {
            lastPosition = transform.position;
            targetPosition = playerPos;
            lerpStartTime = Time.time;
        }
        this.transform.position = Vector2.Lerp(
            lastPosition, State.Position, (Time.time - lerpStartTime) / lerpTime);

        switch (State.Orientation) {
            case Direction.Up:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case Direction.Down:
                transform.rotation = Quaternion.Euler(0, 0, 180);
                break;
            case Direction.Left:
                transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case Direction.Right:
                transform.rotation = Quaternion.Euler(0, 0, 270);
                break;
            default:
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (StateManager != null) {
            // is client side player
            if (!StateManager.GetApproxState().LocalStates.ContainsKey(State.PlayerID)) {
                GameController.PlayerDict.Remove(State.PlayerID);
                Destroy(this.gameObject);
                return;
            }
            updateTransform();
        } else {
            // is server side player
            // update position
            updateTransform();
        }

        if (!State.IsAlive) {
            this.gameObject.SetActive(false);
        }
    }
}
