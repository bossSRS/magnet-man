//// Author: Sadikur Rahman ////
// Configurable settings for magnetic poles: forces, range, speed, and field distance.

using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Pole Settings")]
public class PoleSettings : ScriptableObject {
    [Header("Magnetic Force")]
    [Range(0f, 500f)] public float attarctforceMagnitude = 10f;
    [Range(0f, 500f)] public float repelforceMagnitude = 10f;

    [Header("Movement")]
    [Range(0f, 500f)] public float moveSpeed = 2f;
    [Range(0f, 500f)] public float moveRange = 3f;

    [Header("Magnetic Field Range")]
    [Range(0f, 500f)] public float AreaDistance = 50f;
}