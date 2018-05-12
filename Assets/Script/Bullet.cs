using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public BulletState State = new BulletState();
	// Use this for initialization
	void Start () {

		
	}
	
	// Update is called once per frame
	void Update () {
        if (State == null) return;
        Move();
    }

    public void Move()
    {
        float distance = Time.fixedDeltaTime * Constants.BulletSpeed;
        float x = this.transform.position.x;
        float y = this.transform.position.y;
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
