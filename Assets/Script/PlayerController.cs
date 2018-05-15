using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public StateManager StateManager;
    public PlayerState State = new PlayerState();
    public GameController GameController;
    private static int FireRate = 5;
    float lerpStartTime;
    float lerpTime = 0.5f;
    Vector2 lastPosition = new Vector2();
    Vector2 targetPosition = new Vector2();

	// Use this for initialization
	void Start () {
        
        
    }
	
	// Update is called once per frame
	void Update () {
        if (StateManager != null)
        {
            if (!StateManager.GetApproxState().LocalStates.ContainsKey(State.PlayerID)) {
                GameController.PlayerDict.Remove(State.PlayerID);
                Destroy(this.gameObject);
            } else {
                // player movement
                float distance = Time.fixedDeltaTime * Constants.PlayerSpeed;
                if (Input.GetKey(KeyCode.UpArrow)) {
                    Vector2 pos = move(Direction.Up, distance);
                    StateManager.Move(pos, Direction.Up);
                    Debug.Log(pos);
                } else if (Input.GetKey(KeyCode.DownArrow)) {
                    Vector2 pos = move(Direction.Down, distance);
                    StateManager.Move(pos, Direction.Down);
                    Debug.Log(pos);
                } else if (Input.GetKey(KeyCode.LeftArrow)) {
                    Vector2 pos = move(Direction.Left, distance);
                    StateManager.Move(pos, Direction.Left);
                    Debug.Log(pos);
                } else if (Input.GetKey(KeyCode.RightArrow)) {
                    Vector2 pos = move(Direction.Right, distance);
                    StateManager.Move(pos, Direction.Right);
                    Debug.Log(pos);
                }

                // shoot bullet
                if (Input.GetKeyDown(KeyCode.Space)) {
                    BulletState bulletState = new BulletState
                    {
                        Direction = State.Orientation,
                        Position = InitialBulletPosition()
                    };
                    Debug.Log("Bullet Position: " + InitialBulletPosition());
                    Debug.Log("bullet sended");
                    StateManager.ShootBullet(bulletState);
                }

                // update GameObject
                var playerPos = State.Position;
                transform.position = playerPos;
                // if (playerPos != targetPosition) {
                //     lastPosition = transform.position;
                //     targetPosition = playerPos;
                //     lerpStartTime = Time.time;
                // }
                // this.transform.position = Vector2.Lerp(
                //     lastPosition, State.Position, (Time.time - lerpStartTime) / lerpTime);

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

                if (!State.IsAlive) {
                    this.gameObject.SetActive(false);
                }
            }



        }

    }

    // The initial position of bullet, which is 1 cm away from the player in the direction player is facing
    public Vector2 InitialBulletPosition()
    {
        Vector2 bulletPosition = State.Position;
        float distance = 1;
        float x = this.State.Position.x;
        float y = this.State.Position.y;
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

    // Calculate the next position of the player
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
