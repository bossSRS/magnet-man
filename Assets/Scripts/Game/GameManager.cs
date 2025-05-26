//// Author: Sadikur Rahman ////

using UnityEngine;

public class GameManager : MonoBehaviour {
    [Header("ScriptableObjects")]
    public PlayerSettings playerSettings;
    [Header("Global Data")]
    public static bool isUsingJoystick;
    private void Awake() {
        // Register global SOs and settings
        DIContainer.Register(playerSettings);
    }

    public void Start()
    {
#if !UNITY_EDITOR && UNITY_ANDROID
        Application.targetFrameRate = 60;
#elif UNITY_EDITOR
        Application.targetFrameRate = 165;
#endif
    }
}