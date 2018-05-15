using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public BulletState State = new BulletState();
    public ServerLogic ServerLogic;
    int frame;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        
        if (ServerLogic != null) {
            var globalState = ServerLogic.GlobalState;
            if (!globalState.BulletStates.ContainsKey(State.BulletID)) {
                Debug.Log("Destroy");
                Destroy(this.gameObject);
            }
            // update position from state
            this.transform.position = globalState.BulletStates[State.BulletID].Position;
            Move();
        }
        if (State == null) return;
    }

    public void Move()
    {
        float distance = Time.fixedDeltaTime * Constants.BulletSpeed;
        float x = this.State.Position.x;
        float y = this.State.Position.y;
        // Debug.Log("before:" + transform.position);
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
        // Debug.Log("after:" + transform.position);
        ServerLogic
            .GlobalState
            .BulletStates[State.BulletID]
            .Position = transform.position;

        // destroy bullet if out of range
        if (transform.position.x > 100f || transform.position.x < -100f ||
            transform.position.y > 100f || transform.position.y < -100f) {
            ServerLogic.GlobalState.BulletStates.Remove(State.BulletID);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            int playerID = collision.gameObject.GetComponent<Player>().State.PlayerID;
            string bulletID = transform.parent.gameObject.GetComponent<Bullet>().State.BulletID;
            ServerLogic.OnCollision(bulletID, playerID);
        }
    }
}
