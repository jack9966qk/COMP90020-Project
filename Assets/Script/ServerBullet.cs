using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerBullet : MonoBehaviour {
    public BulletState State = new BulletState();
    public ServerLogic ServerLogic;
	
	// Update is called once per frame
	void Update () {
        if (ServerLogic != null) {
            var globalState = ServerLogic.GlobalState;
            if (!globalState.BulletStates.ContainsKey(State.BulletID)) {
                Debug.Log("Destroy");
                Destroy(this.gameObject);
            } else {
                // update position from state
                this.transform.position = globalState.BulletStates[State.BulletID].Position;
                Move();
            }
        }
        if (State == null) return;
    }

    public void Move() {
        float distance = Time.deltaTime * Constants.BulletSpeed;
        transform.position = DirectionUtil.move(
            State.Position, State.Direction, distance);
        
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) {          
            int playerID = collision.gameObject.GetComponent<Player>().State.PlayerID;
            string bulletID = State.BulletID;
            ServerLogic.OnCollision(bulletID, playerID);
        }
    }

}
