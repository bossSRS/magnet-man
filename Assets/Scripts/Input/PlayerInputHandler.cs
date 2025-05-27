//// Author: Sadikur Rahman ////
// Handles joystick/keyboard input and sends movement data to IMovable. Separate jump and dash interfaces for modularity.

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour {
    [SerializeField] private FixedJoystick joystick;
    [SerializeField] private bool isTesting = false;

    private IMovable mover;
    private IJumpable jumper;
    private IDashable dasher;

    private Vector3 input;

    private void Awake() {
        mover = GetComponent<IMovable>();
        jumper = GetComponent<IJumpable>();
        dasher = GetComponent<IDashable>();
    }

    private void OnEnable() {
        TouchInputReader.OnSwipeUp += TryJump;
        TouchInputReader.OnDoubleTap += TryDash;
    }

    private void OnDisable() {
        TouchInputReader.OnSwipeUp -= TryJump;
        TouchInputReader.OnDoubleTap -= TryDash;
    }

    private void Update() {
        input = isTesting ? GetKeyboardInput() : GetJoystickInput();
        mover?.SetMoveInput(input);
    }

    private Vector3 GetJoystickInput() {
        return new Vector3(joystick.Horizontal, 0f, joystick.Vertical);
    }

    private Vector3 GetKeyboardInput() {
        float h = 0f, v = 0f;
        if (Keyboard.current.leftArrowKey.isPressed) h = -1f;
        if (Keyboard.current.rightArrowKey.isPressed) h = 1f;
        if (Keyboard.current.upArrowKey.isPressed) v = 1f;
        if (Keyboard.current.downArrowKey.isPressed) v = -1f;
        return new Vector3(h, 0f, v);
    }

    private void TryJump() => jumper?.Jump();
    private void TryDash() => dasher?.Dash();
}