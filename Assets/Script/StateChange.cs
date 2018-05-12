using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

public class StateChange : MessageBase {
    public bool HasChange = false;
    // new position
    public Vector2 NewPosition = new Vector2();
    // bullets created
    public HashSet<BulletState> BulletsCreated = new HashSet<BulletState>();

    public void merge(StateChange other) {
        NewPosition = other.NewPosition;
        BulletsCreated.UnionWith(other.BulletsCreated);
    }

    public override void Serialize(NetworkWriter writer) {
        writer.Write(HasChange);
        writer.Write(NewPosition);
        writer.Write(BulletsCreated.Count);
        foreach (var bullet in BulletsCreated) {
            bullet.Serialize(writer);
        }
    }

    public override void Deserialize(NetworkReader reader) {
        var hasChange = reader.ReadBoolean();
        var position = reader.ReadVector2();
        var bullets = new HashSet<BulletState>();
        var numBullet = reader.ReadInt32();
        for (var i = 0; i < numBullet; i++) {
            var bullet = new BulletState();
            bullet.Deserialize(reader);
            bullets.Add(bullet);
        }
        HasChange = hasChange;
        NewPosition = position;
        BulletsCreated = bullets;
    }
}