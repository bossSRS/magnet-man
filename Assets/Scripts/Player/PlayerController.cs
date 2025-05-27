//// Author: Sadikur Rahman ////

using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour, IPlayerMovement {
    public Rigidbody PlayerRigidBody => rb;

    private Rigidbody rb;
    private PlayerSettings settings;
    private PolarityManager polarityManager;

    private Vector3 inputVector;
    public static bool isGrounded;
    private bool canDash = true;

    private Transform stuckPole = null;
    private float repelDamping = 1f; // 1 = full movement, 0 = fully frozen
    private float targetRepelDamping = 1f;
    private const float repelSmoothSpeed = 5f;

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
        repelDamping = Mathf.Lerp(repelDamping, targetRepelDamping, Time.fixedDeltaTime * repelSmoothSpeed);
        Move();
        Rotate();
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

        Vector3 currentVelocity = rb.linearVelocity;
        currentVelocity.y = 0f;
        rb.linearVelocity = currentVelocity;

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


    private void Update() {
#if UNITY_EDITOR
        if (Keyboard.current.leftShiftKey.wasPressedThisFrame) {
            Dash();
        }
#endif
    }
}
