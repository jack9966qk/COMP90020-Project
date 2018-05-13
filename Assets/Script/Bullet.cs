using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public BulletState State = new BulletState();
    public StateManager StateManager;
	// Use this for initialization
	void Start () {

		
	}
	
	// Update is called once per frame
	void Update () {
        if (StateManager != null)
        {
            if (!StateManager.GetApproxState().BulletStates.ContainsKey(State.BulletID))
            {
                Destroy(this.gameObject);
            }
        }
        if (State == null) return;
        Move();
    }

    public void Move()
    {
        float distance = (Time.time-State.SpawnTime) * Constants.BulletSpeed;
        float x = this.State.InitialPosition.x;
        float y = this.State.InitialPosition.y;
        switch (State.Direction)
        {
            case Direction.Up:
                this.transform.position = new Vector2(x, y + distance);
                break;
            case Direction.Down:
                this.transform.position = new Vector2(x, y - distance);
                break;
            case Direction.Left:
                this.transform.position = new Vector2(x - distance, y);
                break;
            case Direction.Right:
                this.transform.position = new Vector2(x + distance, y);
                break;
            default:
                break;
        }
    }
}
