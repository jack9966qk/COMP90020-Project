using UnityEngine;
using System.Collections.Generic;

public class LocalState {
    public PlayerState PlayerState;
    public Dictionary<int, BulletState> BulletStates
        = new Dictionary<int, BulletState>();
}