using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour {
	GlobalState GlobalState
		= new GlobalState();

	public GlobalState GetApproxState() {
		return GlobalState;
	}

	public void SetLocalState(int playerID, LocalState state) {
		GlobalState.PutLocalState(playerID, state);
	}

	public void InitialiseLocalState(int playerID) {
		GlobalState.PutLocalState(playerID, new LocalState());
	}

	public void AddBullet(int playerID, int bulletID, BulletState bulletState) {
		GlobalState
			.GetLocalState(playerID)
			.BulletStates[bulletID] = bulletState;
		// TODO init GameObject
	}

	public void ShootBullet(int playerID, int bulletID) {
		var localState = GlobalState.GetLocalState(playerID);
		var player = localState.PlayerState;
		var bullet = new BulletState(
			new Vector2(player.Position.x, player.Position.y),
			player.Orientation
		);
		AddBullet(playerID, bulletID, bullet);
	}

	void RemoveBullet(int playerID, int bulletID) {
		GlobalState
			.GetLocalState(playerID)
			.BulletStates.Remove(bulletID);
		// removal of GameObject handled by itself
	}

	public void BulletHit(int playerID, int bulletID, int victimID) {
		if (playerID == victimID) return;
		var bullet = GlobalState
			.GetLocalState(playerID)
			.BulletStates[bulletID];
		GlobalState
			.GetLocalState(victimID)
			.PlayerState.Damage(bullet.Damage);
		RemoveBullet(playerID, bulletID);
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
