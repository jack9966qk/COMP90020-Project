using UnityEngine;
using UnityEngine.Networking;

public class StateChangeMessage : MessageBase {
    public Vector2 LogicTime;
    public StateChange StateChange;
}