using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientBullet : MonoBehaviour {
    public BulletState State = new BulletState();
    public GameController GameController;
    public StateManager StateManager;

    float lerpStartTime;
    float lerpTime = 0.5f;
    Vector2 lastPosition = new Vector2();
    Vector2 targetPosition = new Vector2();

	// Use this for initialization
	void Start () {
        
		
	}
	
	// Update is called once per frame
	void Update () {
        if (StateManager != null) {
            var globalState = StateManager.GetApproxState();
            if (!globalState.BulletStates.ContainsKey(State.BulletID)) {
                GameController.BulletDict.Remove(State.BulletID);
             //   Debug.Log("Destroy");
                Destroy(this.gameObject);
            }
            else
            {
                // update position from state
                var bulletPos = globalState.BulletStates[State.BulletID].Position;
             //   Debug.Log("bullet: " + bulletPos);
                transform.position = bulletPos;
                // if (bulletPos != targetPosition)
                // {
                //     lastPosition = transform.position;
                //     targetPosition = bulletPos;
                //     lerpStartTime = Time.time;
                // }
                // this.transform.position = Vector2.Lerp(
                //     lastPosition, State.Position, (Time.time - lerpStartTime) / lerpTime);
            //    Debug.Log("Client Bullet: " + globalState.BulletStates[State.BulletID].Position);
            }

        }
        if (State == null) return;
    }
}
