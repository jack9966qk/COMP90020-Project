using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {
	public StateManager StateManager;
	public int BulletID;

	public void OnTriggerEnter2D(Collider2D other) {
		var victim = other.gameObject.GetComponent<PlayerController>();
		// ...
		// StateManager.BulletHit(PlayerID, BulletID, victim.PlayerID);
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		// TODO change below to fit updated StateManager
		// if (!hasState) {
		// 	this.gameObject.SetActive(false);
		// }

		// float x = State.Position.x;
		// float y = State.Position.y;
		// float z = transform.position.z;
		// transform.position = new Vector3(x, y, z);

		// float distance = Time.fixedDeltaTime * State.Speed;
		// State.Move(distance);
	}
}
