using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletState {
	public Vector2 Position;
	public Direction Direction;
	public float Speed = 10f;
	public float Damage = 30f;

	public BulletState(Vector2 position, Direction direction) {
		this.Position = position;
		this.Direction = direction;
	}

	public void Move(float distance) {
		float x = this.Position.x;
		float y = this.Position.y;
		switch (Direction) {
			case Direction.Up:
				this.Position = new Vector2(x, y + distance);
				break;
			case Direction.Down:
				this.Position = new Vector2(x, y - distance);
				break;
			case Direction.Left:
				this.Position = new Vector2(x - distance, y);
				break;
			case Direction.Right:
				this.Position = new Vector2(x + distance, y);
				break;
			default:
				break;
		}
	}
}
