using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public StateManager StateManager;
    public GameObject RemotePlayerPrefab;
    public GameObject PlayerPrefab;
    public GameObject BulletPrefab;

    public Dictionary<string, GameObject> BulletDict = new Dictionary<string, GameObject>();
    public Dictionary<int, GameObject> PlayerDict = new Dictionary<int, GameObject>();

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
                player.GetComponent<PlayerController>().GameController = this;
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
                player.GetComponent<Player>().GameController = this;
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
            foreach (string bulletID in GlobalState.BulletStates.Keys) {
              //  Debug.Log("Bullet ID: " + bulletID);
                BulletState bulletState = GlobalState.BulletStates[bulletID];
                if (!BulletDict.ContainsKey(bulletID)) {
                    var bullet = Instantiate(BulletPrefab, bulletState.Position, new Quaternion());
                //    Debug.Log("game controller: " + bulletState.Position);
                    bullet.GetComponent<ClientBullet>().State = bulletState;
                    bullet.GetComponent<ClientBullet>().StateManager = StateManager;
                    bullet.GetComponent<ClientBullet>().GameController = this;
                    BulletDict.Add(bulletState.BulletID, bullet);
                } else {
                    BulletDict[bulletID].GetComponent<ClientBullet>().State = bulletState;
                //    Debug.Log("Game Controller Bullet:" +bulletState.Position);
                }
            }
        }


		
	}
}
