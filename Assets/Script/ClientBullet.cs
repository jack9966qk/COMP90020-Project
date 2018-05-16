using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientBullet : MonoBehaviour {
    public BulletState State = new BulletState();
    public GameController GameController;
    public StateManager StateManager;

    Interpolator Interpolator = new Interpolator();

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (StateManager != null) {
            var globalState = StateManager.GetApproxState();
            if (!globalState.BulletStates.ContainsKey(State.BulletID)) {
                GameController.BulletDict.Remove(State.BulletID);
                Destroy(this.gameObject);
            } else {
                // update position from state
                var bulletPos = globalState.BulletStates[State.BulletID].Position;
                transform.position = Interpolator.GetPosition(transform.position, bulletPos);
            }
        }
        if (State == null) return;
    }
}
