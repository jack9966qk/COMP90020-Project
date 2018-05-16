using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public StateManager StateManager;
    public PlayerState State = new PlayerState();
    public GameController GameController;

	// Update is called once per frame
	void Update () {
        // skip if no state manager
        if (StateManager == null) return;
        
        if (!StateManager.GetApproxState().LocalStates.ContainsKey(State.PlayerID)) {
            // destroy object if no longer exist
            GameController.PlayerDict.Remove(State.PlayerID);
            Destroy(this.gameObject);
            return;
        }

        // update colour based on HP
        if (State.HP < Constants.PlayerHP && State.HP > (Constants.PlayerHP / 2)) {
            this.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
        } else if (State.HP <= (Constants.PlayerHP / 2)) {
            this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }

        // player movement
        float distance = Time.deltaTime * Constants.PlayerSpeed;
        if (Input.GetKey(KeyCode.UpArrow)) {
            Vector2 pos = move(Direction.Up, distance);
            StateManager.Move(pos, Direction.Up);
        } else if (Input.GetKey(KeyCode.DownArrow)) {
            Vector2 pos = move(Direction.Down, distance);
            StateManager.Move(pos, Direction.Down);
        } else if (Input.GetKey(KeyCode.LeftArrow)) {
            Vector2 pos = move(Direction.Left, distance);
            StateManager.Move(pos, Direction.Left);
        } else if (Input.GetKey(KeyCode.RightArrow)) {
            Vector2 pos = move(Direction.Right, distance);
            StateManager.Move(pos, Direction.Right);
        }

        // shoot bullet
        if (Input.GetKeyDown(KeyCode.Space)) {
            StateManager.ShootBullet(new BulletState {
                Direction = State.Orientation,
                Position = InitialBulletPosition()
            });
        }

        // update transform
        var playerPos = State.Position;
        transform.position = playerPos;

        switch (State.Orientation) {
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

    // The initial position of bullet,
    // which is 1 unit away from the player
    // in the direction player is facing
    public Vector2 InitialBulletPosition() {
        Vector2 bulletPosition = State.Position;
        float distance = 1;

        return DirectionUtil.move(
            bulletPosition,
            this.State.Orientation,
            distance);
    }

    // Calculate the next position of the player
    public Vector2 move(Direction direction, float distance) {
        float x = this.State.Position.x;
        float y = this.State.Position.y;
        return DirectionUtil.move(this.State.Position, direction, distance);
    }

}
