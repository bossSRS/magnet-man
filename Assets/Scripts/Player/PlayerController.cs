//// Author: Sadikur Rahman ////
// Handles player physics movement, rotation, jumping, and dashing based on IMovable, IJumpable, IDashable interfaces.

using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour, IMovable, IJumpable, IDashable {
    public Rigidbody PlayerRigidBody => rb;

    private Rigidbody rb;
    private PlayerSettings settings;
    private PolarityManager polarityManager;

    private Vector3 inputVector;
    private bool canDash = true;
    public static bool isGrounded;

    private float repelDamping = 1f;
    private float targetRepelDamping = 1f;
    private const float repelSmoothSpeed = 5f;

    private Transform stuckPole;

    [Header("Visuals")]
    [SerializeField] private Renderer faceRenderer;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        settings = DIContainer.Resolve<PlayerSettings>();
        polarityManager = DIContainer.Resolve<PolarityManager>();
    }

    private void Start() {
        UpdateFaceColor(polarityManager.CurrentPolarity);
    }

    private void OnEnable() {
        polarityManager.OnPolarityChanged += UpdateFaceColor;
    }

    private void OnDisable() {
        polarityManager.OnPolarityChanged -= UpdateFaceColor;
    }

    private void FixedUpdate() {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        UpdateRepelDamping();
        Move();
        Rotate();
    }

    private void UpdateRepelDamping() {
        repelDamping = Mathf.Lerp(repelDamping, targetRepelDamping, Time.fixedDeltaTime * repelSmoothSpeed);
    }

    public void SetMoveInput(Vector3 input) => inputVector = input;

    public void Move() {
        Vector3 moveDir = inputVector.normalized * repelDamping;
        Vector3 targetPos = rb.position + moveDir * settings.moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(targetPos);
    }

    public void Rotate() {
        if (inputVector.sqrMagnitude > 0.01f) {
            Quaternion lookRotation = Quaternion.LookRotation(inputVector);
            rb.rotation = Quaternion.Slerp(rb.rotation, lookRotation, Time.fixedDeltaTime * 10f);
        }
    }

    public void Jump() {
        if (!isGrounded) return;

        isGrounded = false;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * settings.jumpForce, ForceMode.Impulse);

        Debug.Log("Jump Applied");
    }

    public void Dash() {
        if (!canDash || inputVector.magnitude == 0f) return;

        canDash = false;

        Vector3 dashDir = inputVector.normalized;
        rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
        rb.AddForce(dashDir * settings.dashForce, ForceMode.VelocityChange);

        Invoke(nameof(ResetDash), 1f);
    }

    private void ResetDash() => canDash = true;

    public void ResetPosition(Vector3 position) {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = position;
    }

    private void UpdateFaceColor(PolarityData polarity) {
        if (faceRenderer != null && polarity != null) {
            faceRenderer.material.color = polarity.color;
        }
    }

    public Vector3 GetInputDirection() => inputVector.normalized;

    public void SetRepelDamping(bool isFreezing) {
        targetRepelDamping = isFreezing ? 0f : 1f;
    }

    public bool IsStuckToPole(Transform pole) => stuckPole == pole;

    private void OnCollisionStay(Collision other) {
        if (other.transform.CompareTag("Pole")) {
            stuckPole = other.transform;
        }
    }

    private void OnCollisionExit(Collision other) {
        if (other.transform.CompareTag("Pole")) {
            stuckPole = null;
        }
    }

#if UNITY_EDITOR
    private void Update() {
        if (Keyboard.current.leftShiftKey.wasPressedThisFrame) {
            Dash();
        }
    }
#endif
}
