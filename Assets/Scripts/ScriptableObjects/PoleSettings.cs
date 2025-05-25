//// Author: Sadikur Rahman ////
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/PoleSettings")]
public class PoleSettings : ScriptableObject {
    public float forceMagnitude = 10f;
    public float moveSpeed = 2f;
    public float moveRange = 3f;
}