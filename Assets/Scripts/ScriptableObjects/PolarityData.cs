//// Author: Sadikur Rahman ////
// Represents a polarity type (North/South) and its visual color.

using UnityEngine;

[CreateAssetMenu(menuName = "Data/Polarity")]
public class PolarityData : ScriptableObject {
    public enum PolarityType { North, South }

    [Header("Polarity")]
    public PolarityType polarity;

    [Header("Visual")]
    public Color color = Color.red;
}