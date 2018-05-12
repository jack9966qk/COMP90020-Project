using System.Collections.Generic;
using UnityEngine;

public class ServerLogic : MonoBehaviour {
	public GlobalState GlobalState;
	public GameObject PlayerPrefab;
	public GameObject BulletPrefab;

	public void Initialise(int numPlayers) {
		var localStates = new Dictionary<int, LocalState>();
		for (var i = 0; i < numPlayers; i++) {
			localStates[i] = new LocalState {
				PlayerState = new PlayerState {
					Position = new Vector2(0, 0),
					Orientation = Direction.Up,
					HP = 100f
				}
			};
			var player = Instantiate(PlayerPrefab, new Vector3(), new Quaternion());
			player.GetComponent<Player>().State = localStates[i].PlayerState;
		}
		GlobalState = new GlobalState {
			LocalStates = localStates,
			BulletStates = new Dictionary<int, BulletState>()
		};
	}

	public void ApplyStateChange(Dictionary<int, StateChange> stateChanges) {
		foreach (var playerId in stateChanges.Keys) {
			var change = stateChanges[playerId];
			// add bullets
			foreach (BulletState bulletState in change.BulletsCreated) {
				// Init the bullets
				var bullet = Instantiate(BulletPrefab, new Vector3(), new Quaternion());
				bullet.GetComponent<Bullet>().State = bulletState;
			}
			// update player position
			var playerState = GlobalState.LocalStates[playerId].PlayerState;
			playerState.Position = change.NewPosition;
		}
	}

	public void OnCollision(int bulletId, int playerId) {
		// apply damage to player
		// var damage = Constants.BulletDamage;
		GlobalState.LocalStates[playerId].PlayerState.Damage(Constants.BulletDamage);
		// remove bullet
		GlobalState.BulletStates.Remove(bulletId);
	}
}