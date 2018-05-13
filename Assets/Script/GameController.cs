﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public StateManager StateManager;
    public GameObject RemotePlayerPrefab;
    public GameObject PlayerPrefab;
    public GameObject BulletPrefab;

    public Dictionary<int, BulletState> BulletDict = new Dictionary<int, BulletState>();
    public Dictionary<int, PlayerState> PlayerDict = new Dictionary<int, PlayerState>();

	// Use this for initialization
	void Start () {


		
	}

    public void Initialise(int[] playerIDs)
    {
        for (int i = 0; i < playerIDs.Length; i++)
        {
            if (ClientNetwork.getPID() == playerIDs[i])
            {
                var localPlayer = Instantiate(PlayerPrefab, new Vector3(), new Quaternion());
                localPlayer.GetComponent<PlayerController>().StateManager = StateManager;
                PlayerState state = new PlayerState
                {
                    Position = new Vector2(0, 0),
                    Orientation = Direction.Up,
                    PlayerID = playerIDs[i],
                    HP = 100f
                };
                localPlayer.GetComponent<PlayerController>().State = state;
                PlayerDict.Add(playerIDs[i], state);
            }
            else
            {
                var player = Instantiate(RemotePlayerPrefab, new Vector3(), new Quaternion());
                player.GetComponent<Player>().StateManager = StateManager;
                PlayerState state = new PlayerState
                {
                    Position = new Vector2(0, 0),
                    Orientation = Direction.Up,
                    PlayerID = playerIDs[i],
                    HP = 100f
                };
                player.GetComponent<Player>().State = state;
                PlayerDict.Add(playerIDs[i], state);
            }
        }

    }


    // Update is called once per frame
    void Update () {
        GlobalState GlobalState = StateManager.GetApproxState();
        // Create remote player if not exists, update existing player states
        foreach(int playerID in GlobalState.LocalStates.Keys)
        {
            PlayerState playerState = GlobalState.LocalStates[playerID].PlayerState;
            if (PlayerDict.ContainsKey(playerID))
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
