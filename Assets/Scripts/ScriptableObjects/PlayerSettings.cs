//// Author: Sadikur Rahman ////
// Contains configurable player physics properties like speed, jump, and dash.

using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Player Settings")]
public class PlayerSettings : ScriptableObject {
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Jump & Dash")]
    public float jumpForce = 7f;
    public float dashForce = 10f;

    [Header("Polarity Timing")]
    public float polarityToggleMin = 3f;
    public float polarityToggleMax = 5f;
}