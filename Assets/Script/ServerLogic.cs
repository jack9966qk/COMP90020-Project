using System.Collections.Generic;
using UnityEngine;

public class ServerLogic : MonoBehaviour {
	GlobalState GlobalState;
	public void ApplyStateChange(Dictionary<int, StateChange> stateChanges) {
		foreach (var playerId in stateChanges.Keys) {
			var change = stateChanges[playerId];
			// add bullets
			foreach (BulletState bulletState in change.BulletsCreated) {
				// TODO init the bullets
			}
			// update player position
			var playerState = GlobalState.GetLocalState(playerId).PlayerState;
			playerState.Position = change.NewPosition;
		}
	}

	public void OnCollision(int bulletId, int playerId) {
		// apply damage to player
		// var damage = Constants.BulletDamage;
		// GlobalState.GetLocalState(playerId).PlayerState.Damage(damage);
		// remove bullet
		GlobalState.RemoveBulletState(bulletId);
	}
}