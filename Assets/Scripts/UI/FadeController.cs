//// Author: Sadikur Rahman ////
// Controls screen fade-in and fade-out using a CanvasGroup for scene transitions.

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeController : MonoBehaviour {
    [Header("Fade Image")]
    [SerializeField] private CanvasGroup fadeGroup;

    private void Awake() {
        DIContainer.Register(this);
    }

    public IEnumerator FadeOut(float duration = 0.5f) {
        yield return Fade(0f, 1f, duration);
    }

    public IEnumerator FadeIn(float duration = 0.5f) {
        yield return Fade(1f, 0f, duration);
    }

    private IEnumerator Fade(float from, float to, float duration) {
        float elapsed = 0f;
        while (elapsed < duration) {
            fadeGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        fadeGroup.alpha = to;
    }
}