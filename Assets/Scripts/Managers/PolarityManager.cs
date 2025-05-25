//// Author: Sadikur Rahman ////

using System;
using UnityEngine;
using System.Collections;
public class PolarityManager : MonoBehaviour {
    public static PolarityManager Instance { get; private set; }

    public PolarityData northPolarity;
    public PolarityData southPolarity;
    public PolarityData CurrentPolarity { get; private set; }

    public event Action<PolarityData> OnPolarityChanged;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DIContainer.Register<PolarityManager>(this);
        CurrentPolarity = northPolarity; // Default start
    }

    private void Start() => StartCoroutine(PolarityCycle());

    private IEnumerator PolarityCycle() {
        while (true) {
            float wait = UnityEngine.Random.Range(3f, 5f);
            yield return new WaitForSeconds(wait);
            TogglePolarity();
        }
    }

    private void TogglePolarity() {
        CurrentPolarity = (CurrentPolarity == northPolarity) ? southPolarity : northPolarity;
        OnPolarityChanged?.Invoke(CurrentPolarity);
    }
}