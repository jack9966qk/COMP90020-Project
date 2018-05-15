using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public PlayerState State;
    public StateManager StateManager;
    public GameController GameController;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (StateManager != null)
        {
            if (!StateManager.GetApproxState().LocalStates.ContainsKey(State.PlayerID))
            {
                GameController.PlayerDict.Remove(State.PlayerID);
                Destroy(this.gameObject);
            }
            else
            {
                if (State == null) return;
                this.transform.position = State.Position;
                switch (State.Orientation)
                {
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

                if (!State.IsAlive)
                {
                    this.gameObject.SetActive(false);
                }

            }

        }

    }
}
