//// Author: Sadikur Rahman ////
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class DashDetector : MonoBehaviour {
    public static event Action OnDash;
    public void OnDashInput(InputAction.CallbackContext ctx) {
        if (ctx.performed) OnDash?.Invoke();
    }
}