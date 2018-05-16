using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public PlayerState State;
    public StateManager StateManager;
    public GameController GameController;
    Interpolator Interpolator = new Interpolator();

	// Use this for initialization
	void Start () {
		
	}

    void updateFromState(bool interpolate) {
        // display different colors based on HP
        if (State.HP < Constants.PlayerHP && State.HP > (Constants.PlayerHP / 2)) {
            this.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        else if (State.HP <= (Constants.PlayerHP / 2)) {
            this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
        
        var playerPos = State.Position;
        transform.position = interpolate ?
            Interpolator.GetPosition(transform.position, playerPos) :
            playerPos;

        switch (State.Orientation) {
            case Direction.Up:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case Direction.Down:
                transform.rotation = Quaternion.Euler(0, 0, 180);
                break;
            case Direction.Left:
                transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case Direction.Right:
                transform.rotation = Quaternion.Euler(0, 0, 270);
                break;
            default:
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (StateManager != null) {
            // is client side player
            if (!StateManager.GetApproxState().LocalStates.ContainsKey(State.PlayerID)) {
                GameController.PlayerDict.Remove(State.PlayerID);
                Destroy(this.gameObject);
                return;
            }
            updateFromState(true);
        } else {
            // is server side player
            updateFromState(false);
        }

        if (!State.IsAlive) {
            this.gameObject.SetActive(false);
        }
    }
}
