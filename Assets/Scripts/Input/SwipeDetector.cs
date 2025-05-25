//// Author: Sadikur Rahman ////

using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class SwipeDetector : MonoBehaviour {
    public static event Action OnSwipeUp;

    private Vector2 startTouch;

    public void OnTouch(InputAction.CallbackContext ctx) {
        print("Detected Touch->Swipe");
        if (ctx.started) {
            startTouch = ctx.ReadValue<Vector2>();
        }
        else if (ctx.canceled) {
            Vector2 endTouch = ctx.ReadValue<Vector2>();
            Vector2 delta = endTouch - startTouch;

            if (delta.y > 50f && Mathf.Abs(delta.y) > Mathf.Abs(delta.x)) {
                OnSwipeUp?.Invoke();
            }
        }
    }
}