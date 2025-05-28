//// Author: Sadikur Rahman ////
// Moves the pole object horizontally using ping-pong motion based on PoleSettings.

using UnityEngine;

public class PoleMovement : MonoBehaviour {
    [SerializeField] private PoleSettings settings;

    private Vector3 startPosition;

    private void Start() {
        startPosition = transform.position;
    }

    private void Update() {
        MoveHorizontally();
    }

    private void MoveHorizontally() {
        float offset = Mathf.PingPong(Time.time * settings.moveSpeed, settings.moveRange) - (settings.moveRange / 2f);
        transform.position = startPosition + new Vector3(offset, 0f, 0f);
    }
}