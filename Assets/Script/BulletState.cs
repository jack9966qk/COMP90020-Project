using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BulletState : MessageBase {
	public Vector2 Position;
	public Direction Direction;
    public int BulletID;

    public override void Serialize(NetworkWriter writer) {
		writer.Write(Position);
		DirectionIO.writeDirectionToBuffer(Direction, writer);
    }

    public override void Deserialize(NetworkReader reader) {
		Position = reader.ReadVector2();
		Direction = DirectionIO.readDirectionFromBuffer(reader);
    }
}
