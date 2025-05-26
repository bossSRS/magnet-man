//// Author: Sadikur Rahman ////

using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour, IPlayerMovement {
    private Rigidbody rb;
    private PlayerSettings settings;
    private PolarityManager polarityManager;

    private Vector3 inputVector;
    public static bool isGrounded;
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
        TouchInputReader.OnSwipeUp += Jump;
        TouchInputReader.OnDoubleTap += Dash;
    }

    private void OnDisable() {
        polarityManager.OnPolarityChanged -= UpdateFaceColor;
        TouchInputReader.OnSwipeUp -= Jump;
        TouchInputReader.OnDoubleTap -= Dash;
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

        isGrounded = false;

        // Reset Y velocity to zero so jump is consistent
        Vector3 currentVelocity = rb.linearVelocity;
        currentVelocity.y = 0f;
        rb.linearVelocity = currentVelocity;

        // Apply upward impulse
        Vector3 jumpForce = Vector3.up * settings.jumpForce;
        rb.AddForce(jumpForce, ForceMode.Impulse);

        Debug.Log("Jump Applied");
    }


    public void Dash() {
        if (!canDash || inputVector.magnitude == 0f) return;

        canDash = false;

        // Fix direction issues
        Vector3 dashDir = inputVector.normalized;

        // Reset vertical velocity for consistent feel
        rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);

        // Apply dash force as velocity (for instant response)
        rb.AddForce(dashDir * settings.dashForce, ForceMode.VelocityChange);

        // Optional: Add dash visual effect or sound here

        Invoke(nameof(ResetDash), 1f); // Cooldown time
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
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = pos;
    }
    
#if UNITY_EDITOR
    private void Update() {
        if (Keyboard.current.leftShiftKey.wasPressedThisFrame) {
            Dash();
        }
    }
#endif
}
