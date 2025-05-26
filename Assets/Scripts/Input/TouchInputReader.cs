//// Author: Sadikur Rahman ////
// Swipe Up (Jump) and Double Tap (Dash) - fixed & instant on mobile/editor

using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class TouchInputReader : MonoBehaviour {
    public static event Action OnSwipeUp;
    public static event Action OnDoubleTap;

    private PlayerControls controls;

    private Vector2 touchStartPos;
    private Vector2 touchEndPos;
    private float lastTapTime = 0f;
    private float doubleTapDelay = 1f;
    private float swipeThreshold = 50f;
    private bool isTouching = false;

    private void Awake() {
        controls = new PlayerControls();

        controls.Player.TouchPress.started += ctx => OnTouchStart();
        controls.Player.TouchPress.canceled += ctx => OnTouchEnd();
        controls.Player.TouchPos.performed += ctx => OnTouchMove(ctx.ReadValue<Vector2>());
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void OnTouchStart() {
        if(GameManager.isUsingJoystick) return;
        isTouching = true;
        touchStartPos = controls.Player.TouchPos.ReadValue<Vector2>();

        float now = Time.time;
        if (now - lastTapTime <= doubleTapDelay) {
            OnDoubleTap?.Invoke();
        }
        lastTapTime = now;
    }

    private void OnTouchMove(Vector2 pos) {
        if (isTouching) {
            touchEndPos = pos;
        }
    }

    private void OnTouchEnd() {
        isTouching = false;
        Vector2 delta = touchEndPos - touchStartPos;

        if (delta.y > swipeThreshold && Mathf.Abs(delta.y) > Mathf.Abs(delta.x)) {
            OnSwipeUp?.Invoke();
        }
    }
}