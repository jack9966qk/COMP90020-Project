using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BulletState : MessageBase {
	public Vector2 InitialPosition;
	public Direction Direction;
	public float SpawnTime;

    public override void Serialize(NetworkWriter writer) {
		writer.Write(InitialPosition);
		DirectionIO.writeDirectionToBuffer(Direction, writer);
		writer.Write(SpawnTime);
    }

    public override void Deserialize(NetworkReader reader) {
		InitialPosition = reader.ReadVector2();
		Direction = DirectionIO.readDirectionFromBuffer(reader);
		SpawnTime = reader.ReadInt32();
    }
}
