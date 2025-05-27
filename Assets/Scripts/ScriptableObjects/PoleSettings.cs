//// Author: Sadikur Rahman ////
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/PoleSettings")]
public class PoleSettings : ScriptableObject {
    [Range(0f,500f)]public float attarctforceMagnitude = 10f;
    [Range(0f,500f)]public float repelforceMagnitude = 10f;
    [Range(0f,500f)]public float moveSpeed = 2f;
    [Range(0f,500f)]public float moveRange = 3f;
    [Range(0f,500f)]public float AreaDistance = 50f;
}