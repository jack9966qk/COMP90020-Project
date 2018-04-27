using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	public PlayerState State;
	public float HP;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (State == null) return;
		HP = State.HP;
		float x = State.Position.x;
		float y = State.Position.y;
		float z = transform.position.z;
		this.transform.position.Set(x, y, z);
		// TODO orientation
	}
}
