using System.Collections.Generic;
using UnityEngine;

public class ServerLogic : MonoBehaviour {
	public GlobalState GlobalState;
	public GameObject PlayerPrefab;
	public GameObject BulletPrefab;

	Dictionary<int, Player> players = new Dictionary<int, Player>();
	Dictionary<int, Bullet> bullets = new Dictionary<int, Bullet>();

	int bulletIdCounter = 0;

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
			players[i] = player.GetComponent<Player>();
		}
		GlobalState = new GlobalState {
			LocalStates = localStates,
			BulletStates = new Dictionary<int, BulletState>()
		};
	}

	public void ApplyStateChange(Dictionary<int, StateChange> stateChanges) {
		Debug.Log(stateChanges.Count);
		foreach (var playerId in stateChanges.Keys) {
			var change = stateChanges[playerId];
			// add bullets
			foreach (BulletState bulletState in change.BulletsCreated) {
				if (!bullets.ContainsKey(bulletState.BulletID)) {
					// Init the bullets
					var bullet = Instantiate(BulletPrefab, bulletState.InitialPosition, new Quaternion());
					bulletState.BulletID = bulletIdCounter;
					bulletIdCounter += 1;
					Debug.Log(bulletState.InitialPosition);
					bullet.GetComponent<Bullet>().State = bulletState;
					bullet.GetComponent<BulletCollision>().serverLogic = this;
					bullets[bulletState.BulletID] = bullet.GetComponent<Bullet>();
					GlobalState.BulletStates[bulletState.BulletID] = bulletState;
				}
			}
			// update player position
			var playerState = GlobalState.LocalStates[playerId].PlayerState;
			if (change.NewPosition.HasValue) {
				playerState.Position = change.NewPosition.Value;
			}
			if (change.NewOrientation.HasValue) {
				playerState.Orientation = change.NewOrientation.Value;
			}
			players[playerId].State = playerState;
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