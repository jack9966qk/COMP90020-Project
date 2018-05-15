using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BulletState : MessageBase {
	public Vector2 Position;
	public Direction Direction;
	public string BulletID;

	public override void Serialize(NetworkWriter writer) {
		writer.Write(Position);
		DirectionIO.writeDirectionToBuffer(Direction, writer);
		writer.Write(BulletID);
	}

	public override void Deserialize(NetworkReader reader) {
		Position = reader.ReadVector2();
		Direction = DirectionIO.readDirectionFromBuffer(reader);
		BulletID = reader.ReadString();
	}
}
