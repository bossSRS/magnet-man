//// Author: Sadikur Rahman ////
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Polarity")]
public class PolarityData : ScriptableObject {
    public enum PolarityType { North, South }
    public PolarityType polarity;
    public Color color;
}