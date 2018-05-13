using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public StateManager StateManager;
    public PlayerState State = new PlayerState();
    private static int FireRate = 5;

	// Use this for initialization
	void Start () {
        //initialise player state

        //StateManager.InitialiseLocalState(PlayerID);
        State.Position = this.transform.position;
        State.Orientation = Direction.Up;
        State.HP = Constants.PlayerHP;
        
    }
	
	// Update is called once per frame
	void Update () {
        if (StateManager != null)
        {
            //Debug.Log("State Manager is not null");
            if (!StateManager.GetApproxState().LocalStates.ContainsKey(State.PlayerID))
            {
                Destroy(this.gameObject);
            }
            // TODO change below to fit updated StateManager
            float distance = Time.fixedDeltaTime * Constants.PlayerSpeed;
            if (Input.GetKey(KeyCode.UpArrow))
            {
                //State.move(Direction.Up, distance);
                Vector2 pos = move(Direction.Up, distance);
                StateManager.Move(pos,Direction.Up);
                Debug.Log(pos);
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                //State.move(Direction.Down, distance);
                Vector2 pos = move(Direction.Down, distance);
                StateManager.Move(pos,Direction.Down);
                Debug.Log(pos);
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                //State.move(Direction.Left, distance);
                Vector2 pos = move(Direction.Left, distance);
                StateManager.Move(pos,Direction.Left);
                Debug.Log(pos);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                Vector2 pos = move(Direction.Right, distance);
                StateManager.Move(pos,Direction.Right);
                Debug.Log(pos);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                //GameObject bullet = GameObject.Instantiate((GameObject)Resources.Load("ClientBullet"), InitialBulletPosition(), this.transform.rotation);
                //bullet.GetComponent<Bullet>().State.Direction = State.Orientation;
                //bullet.GetComponent<Bullet>().State.SpawnTime = Time.time;
                //bullet.GetComponent<Bullet>().State.InitialPosition = InitialBulletPosition();
                BulletState bulletState = new BulletState
                {
                    Direction = State.Orientation,
                    SpawnTime = Time.time,
                    InitialPosition = InitialBulletPosition()

                };
                StateManager.ShootBullet(bulletState);
            }

            // update GameObject
            float x = State.Position.x;
            float y = State.Position.y;
            float z = transform.position.z;
            transform.position = new Vector3(x, y, z);

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

    public Vector2 InitialBulletPosition()
    {
        Vector2 bulletPosition = this.transform.position;
        float distance = 1;
        float x = this.transform.position.x;
        float y = this.transform.position.y;
        switch (this.State.Orientation)
        {
            case Direction.Up:
                bulletPosition = new Vector2(x, y + distance);
                break;
            case Direction.Down:
                bulletPosition = new Vector2(x, y - distance);
                break;
            case Direction.Left:
                bulletPosition = new Vector2(x - distance, y);
                break;
            case Direction.Right:
                bulletPosition = new Vector2(x + distance, y);
                break;
            default:
                break;
        }
        return bulletPosition;
    }

    public Vector2 move(Direction direction, float distance)
    {
        float x = this.State.Position.x;
        float y = this.State.Position.y;
        Vector2 pos = new Vector2();
        switch (direction)
        {
            case Direction.Up:
                pos = new Vector2(x, y + distance);
                break;
            case Direction.Down:
                pos = new Vector2(x, y - distance);
                break;
            case Direction.Left:
                pos = new Vector2(x - distance, y);
                break;
            case Direction.Right:
                pos = new Vector2(x + distance, y);
                break;
            default:
                break;
        }
        return pos;

    }

}
