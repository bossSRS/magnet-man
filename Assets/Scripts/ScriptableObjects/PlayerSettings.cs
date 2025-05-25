//// Author: Sadikur Rahman ////
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/PlayerSettings")]
public class PlayerSettings : ScriptableObject {
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public float dashForce = 10f;
    public float polarityToggleMin = 3f;
    public float polarityToggleMax = 5f;
}