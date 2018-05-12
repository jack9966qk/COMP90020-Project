using UnityEngine;
using UnityEngine.Networking;

public class StateChangeMessage : MessageBase {
    public Vector2 LogicTime;
    public StateChange StateChange;

    public override void Serialize(NetworkWriter writer) {
		writer.Write(LogicTime);
        StateChange.Serialize(writer);
	}

	public override void Deserialize(NetworkReader reader) {
		LogicTime = reader.ReadVector2();
        StateChange = new StateChange();
        StateChange.Deserialize(reader);
	}
}