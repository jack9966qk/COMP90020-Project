using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

public class StateChange {
    // new possition
    public Vector2 NewPosition;
    // bullets created
    public HashSet<BulletState> BulletsCreated;

    public void merge(StateChange other) {
        NewPosition = other.NewPosition;
        BulletsCreated.UnionWith(other.BulletsCreated);
    }

    public void WriteToBuffer(NetworkWriter writer) {
        writer.Write(NewPosition);
        writer.Write(BulletsCreated.Count);
        foreach (var bullet in BulletsCreated) {
            bullet.WriteToBuffer(writer);
        }
    }

    public static StateChange ReadFromBuffer(NetworkReader reader) {
        var position = reader.ReadVector2();
        var bullets = new HashSet<BulletState>();
        var numBullet = reader.ReadInt32();
        for (var i = 0; i < numBullet; i++) {
            bullets.Add(BulletState.ReadFromBuffer(reader));
        }
        return new StateChange {
            NewPosition = position,
            BulletsCreated = bullets
        };
    }
}