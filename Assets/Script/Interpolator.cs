using UnityEngine;

public class Interpolator {
    float lerpStartTime;
    float lerpTime = 0.3f;
    Vector2 lastPosition = new Vector2();
    Vector2 targetPosition = new Vector2();

    public Vector2 GetPosition(Vector2 currentPosition, Vector2 newPosition) {
        // update positions if received new
        if (newPosition != targetPosition) {
            lastPosition = currentPosition;
            targetPosition = newPosition;
            // calculate interpolation time
            var dist = Vector2.Distance(lastPosition, targetPosition);
            lerpTime = dist / Constants.BulletSpeed;
            lerpStartTime = Time.time - Time.deltaTime;
        }
        // do interpolation
        return Vector2.Lerp(
           lastPosition, newPosition, (Time.time - lerpStartTime) / lerpTime);
    }
}