//// Author: Sadikur Rahman ////
// Smooth camera follow with adjustable damping and rotation (if needed)

using UnityEngine;

public class CameraFollow : MonoBehaviour {
    [Header("Target Settings")]
    [SerializeField] private Transform target;

    [Header("Position Offset")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 5f, -7f);

    [Header("Smooth Follow")]
    [SerializeField] private float positionDamping = 0.15f;
    private Vector3 velocity = Vector3.zero;

    [Header("Optional Rotation")]
    [SerializeField] private bool followRotation = false;
    [SerializeField] private float rotationSpeed = 5f;

    private void FixedUpdate() {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;

        // Smooth damp for more natural acceleration/deceleration
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, positionDamping);

        // Optional: Smooth rotate camera to match target
        if (followRotation) {
            Quaternion targetRot = Quaternion.LookRotation(target.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }
    }

    public void SetTarget(Transform newTarget) {
        target = newTarget;
    }
}