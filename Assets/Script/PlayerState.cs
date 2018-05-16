using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerState : MessageBase {
	public Vector2 Position;
	public Direction Orientation;
    public Boolean Stationary;
	public float HP;
    public int PlayerID;

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

    public override void Serialize(NetworkWriter writer) {
		writer.Write(Position);
		DirectionIO.writeDirectionToBuffer(Orientation, writer);
		writer.Write(HP);
        writer.Write(Stationary);
    }

    public override void Deserialize(NetworkReader reader) {
		Position = reader.ReadVector2();
		Orientation = DirectionIO.readDirectionFromBuffer(reader);
		HP = reader.ReadSingle();
        Stationary = reader.ReadBoolean();
    }
}

