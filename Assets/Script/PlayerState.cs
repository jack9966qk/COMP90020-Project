using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerState  {
	public Vector2 Position { get; private set; }
	public Direction Orientation { get; private set; }
	public float HP { get; private set; }

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

    public void WriteToBuffer(NetworkWriter writer) {
		writer.Write(Position);
		DirectionIO.writeDirectionToBuffer(Orientation, writer);
		writer.Write(HP);
    }

    public static PlayerState ReadFromBuffer(NetworkReader reader) {
        return new PlayerState {
			Position = reader.ReadVector2(),
			Orientation = DirectionIO.readDirectionFromBuffer(reader),
			HP = reader.ReadSingle()
		};
    }
}

