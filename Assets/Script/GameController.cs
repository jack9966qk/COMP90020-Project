using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public StateManager StateManager;
    public GameObject RemotePlayerPrefab;
    public GameObject PlayerPrefab;
    public GameObject BulletPrefab;

    public Dictionary<int, BulletState> BulletDict;
    public Dictionary<int, PlayerState> PlayerDict;

	// Use this for initialization
	void Start () {

		
	}
	
	// Update is called once per frame
	void Update () {
        GlobalState GlobalState = StateManager.GetApproxState();
        // Create remote player if not exists, update existing player states
        foreach(int playerID in GlobalState.LocalStates.Keys)
        {
            PlayerState playerState = GlobalState.LocalStates[playerID].PlayerState;
            if (!PlayerDict.ContainsKey(playerID))
            {
                var player = Instantiate(RemotePlayerPrefab, new Vector3(), new Quaternion());
                player.GetComponent<Player>().State = playerState;
                player.GetComponent<Player>().StateManager = StateManager;
                PlayerDict.Add(playerState.PlayerID, playerState);
            }
            else
            {
                PlayerDict[playerID] = playerState;
            }
        }
        // Create bullets if not exists, update existing bullet State
        foreach(int bulletID in GlobalState.BulletStates.Keys)
        {
            BulletState bulletState = GlobalState.BulletStates[bulletID];
            if (!BulletDict.ContainsKey(bulletID))
            {
                var bullet = Instantiate(BulletPrefab, new Vector3(), new Quaternion());
                bullet.GetComponent<Bullet>().State = bulletState;
                bullet.GetComponent<Bullet>().StateManager = StateManager;
                BulletDict.Add(bulletState.BulletID, bulletState);
            }
            else
            {
                BulletDict[bulletID] = bulletState;
            }
        }

		
	}
}
