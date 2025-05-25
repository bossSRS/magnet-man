//// Author: Sadikur Rahman ////
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class SwipeDetector : MonoBehaviour {
    public static event Action OnSwipeUp;
    private Vector2 start;

    public void OnTouch(InputAction.CallbackContext ctx) {
        if (ctx.started) start = ctx.ReadValue<Vector2>();
        else if (ctx.canceled) {
            Vector2 end = ctx.ReadValue<Vector2>();
            if ((end - start).y > 50f) OnSwipeUp?.Invoke();
        }
    }
}