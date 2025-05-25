//// Author: Sadikur Rahman ////

using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class DashDetector : MonoBehaviour {
    public static event Action OnDash;

    private float lastTapTime = 0f;
    private const float doubleTapThreshold = 0.4f;

    public void OnTouch(InputAction.CallbackContext ctx) {
        print("Detected Touch->Dash");
        if (ctx.performed) {
            float currentTime = Time.time;
            if (currentTime - lastTapTime <= doubleTapThreshold) {
                OnDash?.Invoke();
            }
            lastTapTime = currentTime;
        }
    }
}