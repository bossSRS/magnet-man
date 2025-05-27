//// Author: Sadikur Rahman ////
// Handles swipe up (Jump) and double tap (Dash) using Unity Input System for touch or mouse input.

using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class TouchInputReader : MonoBehaviour {
    public static event Action OnSwipeUp;
    public static event Action OnDoubleTap;

    private PlayerControls controls;
    private Vector2 touchStartPos, touchEndPos;

    private float lastTapTime;
    private const float doubleTapDelay = 1f;
    private const float swipeThreshold = 50f;

    private bool isTouching;

    private void Awake() {
        controls = new PlayerControls();

        controls.Player.TouchPress.started += _ => HandleTouchStart();
        controls.Player.TouchPress.canceled += _ => HandleTouchEnd();
        controls.Player.TouchPos.performed += ctx => touchEndPos = ctx.ReadValue<Vector2>();
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void HandleTouchStart() {
        if (GameManager.isUsingJoystick) return;

        isTouching = true;
        touchStartPos = controls.Player.TouchPos.ReadValue<Vector2>();

        if (Time.time - lastTapTime <= doubleTapDelay) {
            OnDoubleTap?.Invoke();
        }

        lastTapTime = Time.time;
    }

    private void HandleTouchEnd() {
        if (GameManager.isUsingJoystick) return;

        isTouching = false;
        Vector2 delta = touchEndPos - touchStartPos;

        if (delta.y > swipeThreshold && Mathf.Abs(delta.y) > Mathf.Abs(delta.x)) {
            OnSwipeUp?.Invoke();
        }
    }
}