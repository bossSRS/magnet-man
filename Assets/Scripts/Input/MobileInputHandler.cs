//// Author: Sadikur Rahman ////
// Combines swipe detection for jump and double tap for dash

using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class MobileInputHandler : MonoBehaviour {
    public static event Action OnSwipeUp;
    public static event Action OnDoubleTap;

    private Vector2 touchStartPos;
    private float lastTapTime = 0f;
    private const float doubleTapThreshold = 0.4f;
    private const float minSwipeDistance = 50f;

    public void OnTouch(InputAction.CallbackContext ctx) {
        if (ctx.started) {
            // First touch
            touchStartPos = ctx.ReadValue<Vector2>();

            // Detect double tap
            float currentTime = Time.time;
            if (currentTime - lastTapTime <= doubleTapThreshold) {
                OnDoubleTap?.Invoke();
            }
            lastTapTime = currentTime;
        }
        else if (ctx.canceled) {
            // End of touch — check for swipe
            Vector2 endPos = ctx.ReadValue<Vector2>();
            Vector2 delta = endPos - touchStartPos;

            if (delta.y > minSwipeDistance && Mathf.Abs(delta.y) > Mathf.Abs(delta.x)) {
                OnSwipeUp?.Invoke();
            }
        }
    }
}