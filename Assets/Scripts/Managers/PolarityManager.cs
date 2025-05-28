//// Author: Sadikur Rahman ////
// Manages current magnetic polarity and toggles it periodically. Broadcasts changes to listeners.

using System;
using System.Collections;
using UnityEngine;

public class PolarityManager : MonoBehaviour {
    public static PolarityManager Instance { get; private set; }

    [Header("Polarity Settings")]
    [SerializeField] private PolarityData northPolarity;
    [SerializeField] private PolarityData southPolarity;
    private PlayerSettings playerSettings;
    public PolarityData CurrentPolarity { get; private set; }

    public event Action<PolarityData> OnPolarityChanged;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DIContainer.Register(this);
        CurrentPolarity = northPolarity; // Initial state
        playerSettings = DIContainer.Resolve<PlayerSettings>();
    }

    private void Start() {
        StartCoroutine(PolarityCycle());
    }

    private IEnumerator PolarityCycle() {
        while (true) {
            float waitTime = UnityEngine.Random.Range(playerSettings.polarityToggleMin, playerSettings.polarityToggleMax);
            yield return new WaitForSeconds(waitTime);
            TogglePolarity();
        }
    }

    private void TogglePolarity() {
        CurrentPolarity = (CurrentPolarity == northPolarity) ? southPolarity : northPolarity;
        OnPolarityChanged?.Invoke(CurrentPolarity);
    }
}