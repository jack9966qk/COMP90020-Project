using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

public class StateChange : MessageBase {
    public bool HasChange = false;
    // new position
    public Vector2 NewPosition = new Vector2();
    // new orientation
    public Direction NewOrientation = Direction.Up;
    // bullets created
    public HashSet<BulletState> BulletsCreated = new HashSet<BulletState>();

    public void merge(StateChange other) {
        HasChange = HasChange || other.HasChange;
        NewPosition = other.NewPosition;
        NewOrientation = other.NewOrientation;
        BulletsCreated.UnionWith(other.BulletsCreated);
    }

    public override void Serialize(NetworkWriter writer) {
        writer.Write(HasChange);
        writer.Write(NewPosition);
        DirectionIO.writeDirectionToBuffer(NewOrientation, writer);
        writer.Write(BulletsCreated.Count);
        foreach (var bullet in BulletsCreated) {
            bullet.Serialize(writer);
        }
    }

    public override void Deserialize(NetworkReader reader) {
        var hasChange = reader.ReadBoolean();
        var position = reader.ReadVector2();
        var orientation = DirectionIO.readDirectionFromBuffer(reader);
        var bullets = new HashSet<BulletState>();
        var numBullet = reader.ReadInt32();
        for (var i = 0; i < numBullet; i++) {
            var bullet = new BulletState();
            bullet.Deserialize(reader);
            bullets.Add(bullet);
        }
        HasChange = hasChange;
        NewPosition = position;
        NewOrientation = orientation;
        BulletsCreated = bullets;
    }
}