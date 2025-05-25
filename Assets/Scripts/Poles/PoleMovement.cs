//// Author: Sadikur Rahman ////
using UnityEngine;

public class PoleMovement : MonoBehaviour {
    public PoleSettings settings;
    private Vector3 startPos;

    private void Start() {
        startPos = transform.position;
    }

    private void Update() {
        float x = Mathf.PingPong(Time.time * settings.moveSpeed, settings.moveRange) - (settings.moveRange / 2);
        transform.position = startPos + new Vector3(x, 0, 0);
    }
}