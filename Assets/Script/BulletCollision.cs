using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour {

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
            collision.gameObject.GetComponent<Player>().State.HP -= Constants.BulletDamage;
            Destroy(collision.gameObject);
        }
        else
        {
            Destroy(collision.gameObject);
        }
        Destroy(transform.parent.gameObject);
    }
}
