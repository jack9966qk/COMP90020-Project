using System;
using UnityEngine.Networking;

public enum Direction {
	Up, Down, Left, Right
}

public class DirectionIO {
	public static void writeDirectionToBuffer(Direction d, NetworkWriter writer) {
		switch (d) {
			case Direction.Up:
				writer.Write(0);
				break;
			case Direction.Down:
				writer.Write(1);
				break;
			case Direction.Left:
				writer.Write(2);
				break;
			case Direction.Right:
				writer.Write(3);
				break;
			default:
				throw new Exception();
		}
	}

	public static Direction readDirectionFromBuffer(NetworkReader reader) {
		switch (reader.ReadInt32()) {
			case 0:
				return Direction.Up;
			case 1:
				return Direction.Down;
			case 2:
				return Direction.Left;
			case 3:
				return Direction.Right;
			default:
				throw new Exception();
		}
	}
}