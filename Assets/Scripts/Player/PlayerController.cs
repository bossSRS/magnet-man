//// Author: Sadikur Rahman ////

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour, IPlayerMovement {
    private Rigidbody rb;
    private PlayerSettings settings;
    private PolarityManager polarityManager;

    private Vector3 inputVector;
    private bool isGrounded;
    private bool canDash = true;

    [Header("Visuals")]
    public Renderer faceRenderer;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

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
        SwipeDetector.OnSwipeUp += Jump;
        DashDetector.OnDash += Dash;
    }

    private void OnDisable() {
        polarityManager.OnPolarityChanged -= UpdateFaceColor;
        SwipeDetector.OnSwipeUp -= Jump;
        DashDetector.OnDash -= Dash;
    }

    private void FixedUpdate() {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        Move();
        Rotate();
    }

    public void SetMoveInput(Vector3 input) => inputVector = input;

    public void Move() {
        Vector3 moveDir = inputVector.normalized;
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

        rb.AddForce(Vector3.up * settings.jumpForce, ForceMode.Impulse);
    }

    public void Dash() {
        if (!canDash || inputVector.magnitude == 0f) return;

        canDash = false;
        Vector3 dashDir = inputVector.normalized;
        rb.AddForce(dashDir * settings.dashForce, ForceMode.Impulse);
        Invoke(nameof(ResetDash), 1f); // Adjust cooldown as needed
    }

    private void ResetDash() {
        canDash = true;
    }

    private void UpdateFaceColor(PolarityData polarity) {
        if (faceRenderer != null && polarity != null) {
            faceRenderer.material.color = polarity.color;
        }
    }

    public void ResetPosition(Vector3 pos) {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = pos;
    }
}
