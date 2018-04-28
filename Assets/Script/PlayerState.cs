using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState  {
	public enum Direction {
		Up, Down, Left, Right
	}
	public Vector2 Position { get; private set; }
	public Direction Orientation { get; private set; }
	public float HP { get; private set; }

	public PlayerState(float x, float y, Direction orientation, float hp) {
		Position = new Vector2(x, y);
		Orientation = orientation;
		HP = hp;
	}

	public bool IsAlive {
		get {
			return this.HP > 0;
		}
	}

	public void Damage(float damage) {
		this.HP = Mathf.Max(0f, this.HP - damage);
	}

	public void move(Direction direction, float distance) {
		float x = this.Position.x;
		float y = this.Position.y;
		this.Orientation = direction;
		switch (direction) {
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

