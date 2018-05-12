using UnityEngine;
using UnityEngine.Networking;

public class GlobalStateMessage : MessageBase {
    public Vector2 LogicTime;
    public GlobalState GlobalState;

    public override void Serialize(NetworkWriter writer) {
		writer.Write(LogicTime);
        GlobalState.Serialize(writer);
	}

	public override void Deserialize(NetworkReader reader) {
		LogicTime = reader.ReadVector2();
        GlobalState = new GlobalState();
        GlobalState.Deserialize(reader);
	}
}