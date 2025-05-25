//// Author: Sadikur Rahman ////

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeController : MonoBehaviour {
    public CanvasGroup fadeGroup;

    private void Awake() {
        DIContainer.Register(this); // Register itself globally
    }

    public IEnumerator FadeOut(float duration = 0.5f) {
        for (float t = 0; t < duration; t += Time.deltaTime) {
            fadeGroup.alpha = Mathf.Lerp(0, 1, t / duration);
            yield return null;
        }
        fadeGroup.alpha = 1;
    }

    public IEnumerator FadeIn(float duration = 0.5f) {
        for (float t = 0; t < duration; t += Time.deltaTime) {
            fadeGroup.alpha = Mathf.Lerp(1, 0, t / duration);
            yield return null;
        }
        fadeGroup.alpha = 0;
    }
}