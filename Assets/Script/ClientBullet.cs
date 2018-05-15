using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientBullet : MonoBehaviour {
    public BulletState State = new BulletState();
    public StateManager StateManager;
	// Use this for initialization
	void Start () {
        
		
	}
	
	// Update is called once per frame
	void Update () {
        if (StateManager != null) {
            var globalState = StateManager.GetApproxState();
            if (!globalState.BulletStates.ContainsKey(State.BulletID)) {
                Debug.Log("Destroy");
                Destroy(this.gameObject);
            }
            // update position from state
            this.transform.position = globalState.BulletStates[State.BulletID].Position;
            Debug.Log("Client Bullet: "+globalState.BulletStates[State.BulletID].Position);
        }
        if (State == null) return;
    }
}
