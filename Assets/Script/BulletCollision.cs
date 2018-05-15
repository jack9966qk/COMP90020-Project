using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour {
    public ServerLogic serverLogic;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            int playerID = collision.gameObject.GetComponent<Player>().State.PlayerID;
            string bulletID = transform.parent.gameObject.GetComponent<Bullet>().State.BulletID;
            serverLogic.OnCollision(bulletID, playerID);
        }
    }
}
