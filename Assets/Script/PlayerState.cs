using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState  {
	public enum Direction {
		Up, Down, Left, Right
	}
	public Vector2 Position;
	public Direction Orientation;
	public float HP;
}

