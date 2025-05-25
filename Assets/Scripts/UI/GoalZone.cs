//// Author: Sadikur Rahman ////

using UnityEngine;
using System.Collections;

public class GoalZone : MonoBehaviour {
    private FadeController fadeUI;
    [SerializeField] private Transform respawnPoint;

    private void Start() {
        fadeUI = DIContainer.Resolve<FadeController>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            var player = other.GetComponent<PlayerController>();
            StartCoroutine(ResetPlayer(player));
        }
    }

    private IEnumerator ResetPlayer(PlayerController player) {
        yield return fadeUI.FadeOut();
        player.ResetPosition(respawnPoint.position);
        yield return fadeUI.FadeIn();
    }
}