using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour {
	GlobalState GlobalState
		= new GlobalState();

	public GlobalState GetApproxState() {
		return GlobalState;
	}

	public void Move(Vector2 pos) {
		//...
	}

	public void ShootBullet() {
		//...
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		// do approximation
		// also apply transition (smoothing)
		// update gloabl objects (bullets)
	}
}
