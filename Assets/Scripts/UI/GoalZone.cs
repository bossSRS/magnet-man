//// Author: Sadikur Rahman ////
// Handles player reaching the goal zone. Fades out, resets position, and fades in.

using UnityEngine;
using System.Collections;

public class GoalZone : MonoBehaviour {
    [Header("Re-Spawn Point")]
    [SerializeField] private Transform respawnPoint;

    private FadeController fadeUI;

    private void Start() {
        fadeUI = DIContainer.Resolve<FadeController>();
    }

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) return;

        var player = other.GetComponent<PlayerController>();
        if (player != null) {
            StartCoroutine(HandleGoalReached(player));
        }
    }

    private IEnumerator HandleGoalReached(PlayerController player) {
        yield return fadeUI.FadeOut();
        player.ResetPosition(respawnPoint.position);
        yield return fadeUI.FadeIn();
    }
}