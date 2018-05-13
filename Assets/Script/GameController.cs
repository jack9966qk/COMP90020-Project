using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public StateManager StateManager;
    public GameObject RemotePlayerPrefab;
    public GameObject PlayerPrefab;
    public GameObject BulletPrefab;

    public Dictionary<int, GameObject> BulletDict = new Dictionary<int, GameObject>();
    public Dictionary<int, GameObject> PlayerDict = new Dictionary<int, GameObject>();
    //public PlayerController localPlayer = new PlayerController();

	// Use this for initialization
	void Start () {


		
	}

    public void Initialise(IEnumerable<int> playerIDs)
    {
        foreach (var playerID in playerIDs) {
            if (ClientNetwork.getPID() == playerID) {
                var player = Instantiate(PlayerPrefab, new Vector3(), new Quaternion());
                player.GetComponent<PlayerController>().StateManager = StateManager;
                PlayerState state = new PlayerState
                {
                    Position = new Vector2(0, 0),
                    Orientation = Direction.Up,
                    PlayerID = playerID,
                    HP = 100f
                };
                player.GetComponent<PlayerController>().State = state;
                PlayerDict.Add(playerID, player);
            } else {
                var player = Instantiate(RemotePlayerPrefab, new Vector3(), new Quaternion());
                player.GetComponent<Player>().StateManager = StateManager;
                PlayerState state = new PlayerState
                {
                    Position = new Vector2(0, 0),
                    Orientation = Direction.Up,
                    PlayerID = playerID,
                    HP = 100f
                };
                player.GetComponent<Player>().State = state;
                PlayerDict.Add(playerID, player);
            }
        }

    }


    // Update is called once per frame
    void Update () {
        GlobalState GlobalState = StateManager.GetApproxState();
        if (GlobalState != null)
        {
            // Create remote player if not exists, update existing player states
            foreach (int playerID in GlobalState.LocalStates.Keys)
            {
                PlayerState playerState = GlobalState.LocalStates[playerID].PlayerState;
                if (PlayerDict.ContainsKey(playerID))
                {
                    if (playerID == ClientNetwork.getPID())
                    {
                        PlayerDict[playerID].GetComponent<PlayerController>().State= playerState;
                    }
                    else
                    {
                        PlayerDict[playerID].GetComponent<Player>().State = playerState;
                    }
                    

                }
            }
            // Create bullets if not exists, update existing bullet State
            foreach (int bulletID in GlobalState.BulletStates.Keys)
            {
                BulletState bulletState = GlobalState.BulletStates[bulletID];
                if (!BulletDict.ContainsKey(bulletID))
                {
                    var bullet = Instantiate(BulletPrefab, new Vector3(), new Quaternion());
                    bullet.GetComponent<Bullet>().State = bulletState;
                    bullet.GetComponent<Bullet>().StateManager = StateManager;
                    BulletDict.Add(bulletState.BulletID, bullet);
                }
                else
                {
                    BulletDict[bulletID].GetComponent<Bullet>().State = bulletState;
                }
            }
        }


		
	}
}
