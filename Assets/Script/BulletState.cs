using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BulletState {
	public Vector2 InitialPosition;
	public Direction Direction;
	public int SpawnTime;
	
	// public Vector2 Position;
	// public Direction Direction;
	// public float Speed = 10f;
	// public float Damage = 30f;

	// public void Move(float distance) {
	// 	float x = this.Position.x;
	// 	float y = this.Position.y;
	// 	switch (Direction) {
	// 		case Direction.Up:
	// 			this.Position = new Vector2(x, y + distance);
	// 			break;
	// 		case Direction.Down:
	// 			this.Position = new Vector2(x, y - distance);
	// 			break;
	// 		case Direction.Left:
	// 			this.Position = new Vector2(x - distance, y);
	// 			break;
	// 		case Direction.Right:
	// 			this.Position = new Vector2(x + distance, y);
	// 			break;
	// 		default:
	// 			break;
	// 	}
	// }

    public void WriteToBuffer(NetworkWriter writer) {
		writer.Write(InitialPosition);
		DirectionIO.writeDirectionToBuffer(Direction, writer);
		writer.Write(SpawnTime);
    }

    public static BulletState ReadFromBuffer(NetworkReader reader) {
		return new BulletState {
			InitialPosition = reader.ReadVector2(),
			Direction = DirectionIO.readDirectionFromBuffer(reader),
			SpawnTime = reader.ReadInt32()
		};
    }
}
