//// Author: Sadikur Rahman ////

using UnityEngine;

public class CameraFollow : MonoBehaviour {
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0, 5, -7);
    [SerializeField] private float smoothSpeed = 5f;

    private void LateUpdate() {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.smoothDeltaTime);
    }

    public void SetTarget(Transform newTarget) {
        target = newTarget;
    }
}