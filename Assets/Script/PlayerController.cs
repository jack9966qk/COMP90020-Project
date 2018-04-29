using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public StateManager StateManager;
	public int PlayerID;
	PlayerState State {
		get {
			return StateManager
				.GetApproxState()
				.GetLocalState(PlayerID)
				.PlayerState;
		} set {
			var LocalState = StateManager
				.GetApproxState()
				.GetLocalState(PlayerID);
			LocalState.PlayerState = value;
			StateManager.SetLocalState(PlayerID, LocalState);
		}
	}
	public float MoveSpeed;

	// Use this for initialization
	void Start () {
		// initialise player state
		StateManager.InitialiseLocalState(PlayerID);
		State = new PlayerState(
			transform.position.x,
			transform.position.y,
			Direction.Up,
			100f);
	}
	
	// Update is called once per frame
	void Update () {
		float distance = Time.fixedDeltaTime * MoveSpeed;
		if (Input.GetKey(KeyCode.UpArrow)) {
			State.move(Direction.Up, distance);
		} else if (Input.GetKey(KeyCode.DownArrow)) {
			State.move(Direction.Down, distance);
		} else if (Input.GetKey(KeyCode.LeftArrow)) {
			State.move(Direction.Left, distance);
		} else if (Input.GetKey(KeyCode.RightArrow)) {
			State.move(Direction.Right, distance);
		}

		if (Input.GetKeyDown(KeyCode.Space)) {
			// TODO bullet
		}

		// update GameObject
		float x = State.Position.x;
		float y = State.Position.y;
		float z = transform.position.z;
		transform.position = new Vector3(x, y, z);

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

		if (!State.IsAlive) {
			this.gameObject.SetActive(false);
		}
	}

}
