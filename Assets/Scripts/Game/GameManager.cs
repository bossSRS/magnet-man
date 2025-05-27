//// Author: Sadikur Rahman ////
// Initializes global game settings and registers ScriptableObjects into the DI container.

using UnityEngine;

public class GameManager : MonoBehaviour {
    [Header("ScriptableObjects")]
    [SerializeField] private PlayerSettings playerSettings;

    public static bool isUsingJoystick;

    private void Awake() {
        DIContainer.Register(playerSettings);
        SetTargetFrameRate();
    }

    private void SetTargetFrameRate() {
#if UNITY_ANDROID && !UNITY_EDITOR
        Application.targetFrameRate = 60;
#else
        Application.targetFrameRate = 165;
#endif
    }
}