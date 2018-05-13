using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

public class StateChange : MessageBase {
    // new position
    public Vector2? NewPosition = null;
    // new orientation
    public Direction? NewOrientation = null;
    // bullets created
    public HashSet<BulletState> BulletsCreated = new HashSet<BulletState>();

    public void merge(StateChange other) {
        if (other.NewPosition.HasValue) {
            NewPosition = other.NewPosition;
        }
        if (other.NewOrientation.HasValue) {
            NewOrientation = other.NewOrientation;
        }
        BulletsCreated.UnionWith(other.BulletsCreated);
    }

    public override void Serialize(NetworkWriter writer) {
        writer.Write(NewPosition.HasValue);
        if (NewPosition.HasValue) {
            writer.Write(NewPosition.Value);
        }
        writer.Write(NewOrientation.HasValue);
        if (NewOrientation.HasValue) {
            DirectionIO.writeDirectionToBuffer(NewOrientation.Value, writer);
        }
        writer.Write(BulletsCreated.Count);
        foreach (var bullet in BulletsCreated) {
            bullet.Serialize(writer);
        }
    }

    public override void Deserialize(NetworkReader reader) {
        Vector2? position = null;
        var hasNewPosition = reader.ReadBoolean();
        if (hasNewPosition) {
            position = reader.ReadVector2();
        }
        Direction? orientation = null;
        var hasNewOrientation = reader.ReadBoolean();
        if (hasNewOrientation) {
            orientation = DirectionIO.readDirectionFromBuffer(reader);
        }
        var bullets = new HashSet<BulletState>();
        var numBullet = reader.ReadInt32();
        for (var i = 0; i < numBullet; i++) {
            var bullet = new BulletState();
            bullet.Deserialize(reader);
            bullets.Add(bullet);
        }
        NewPosition = position;
        NewOrientation = orientation;
        BulletsCreated = bullets;
    }
}