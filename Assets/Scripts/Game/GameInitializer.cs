//// Author: Sadikur Rahman ////

using UnityEngine;

public class GameInitializer : MonoBehaviour {
    [Header("ScriptableObjects")]
    public PlayerSettings playerSettings;
    private void Awake() {
        // Register global SOs and settings
        DIContainer.Register(playerSettings);
    }

    public void Start()
    {
        Application.targetFrameRate = 60;
    }
}