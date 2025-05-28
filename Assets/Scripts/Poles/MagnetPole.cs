//// Author: Sadikur Rahman ////
// Applies polarity-based magnetic effects to the player: attraction or repulsion.

using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MagnetPole : MonoBehaviour {
    [Header("Polarity & Settings")]
    public PolarityData polePolarity;
    public PoleSettings settings;

    private PolarityManager polarityManager;
    private PlayerController playerController;

    [Header("Debug Info")]
    public bool isPolarityInEffect;
    private bool wasInPolarityZone;

    private void Start() {
        polarityManager = DIContainer.Resolve<PolarityManager>();
        playerController = FindObjectOfType<PlayerController>();
    }

    private void Update() {
        CheckPolarityEffectZone();
    }

    private void FixedUpdate() {
        if (!isPolarityInEffect || playerController?.PlayerRigidBody == null) return;

        Vector3 direction = playerController.transform.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f) return;

        Vector3 dirNormalized = direction.normalized;
        bool samePolarity = polePolarity.polarity == polarityManager.CurrentPolarity.polarity;
        float inputDot = Vector3.Dot(playerController.GetInputDirection(), -dirNormalized);

        if (samePolarity)
            ApplyRepelEffect(dirNormalized, inputDot);
        else
            ApplyAttractEffect(dirNormalized);
    }

    private void CheckPolarityEffectZone() {
        float distance = Vector3.Distance(transform.position, playerController.transform.position);
        isPolarityInEffect = distance <= settings.AreaDistance;

        if (wasInPolarityZone && !isPolarityInEffect) {
            playerController.SetRepelDamping(false); // Smooth restore
        }

        wasInPolarityZone = isPolarityInEffect;
    }

    private void ApplyAttractEffect(Vector3 dirNormalized) {
        var rb = playerController.PlayerRigidBody;
        float force = settings.attarctforceMagnitude;

        if (!playerController.IsStuckToPole(transform)) {
            Vector3 attract = -dirNormalized * force;
            rb.AddForceAtPosition(attract, transform.position, ForceMode.Force);
        } else {
            // Lock to surface gently
            Vector3 softAttach = -dirNormalized;
            rb.MovePosition(rb.position + softAttach * Time.fixedDeltaTime);
        }

        playerController.SetRepelDamping(false);
    }

    private void ApplyRepelEffect(Vector3 dirNormalized, float inputDot) {
        var rb = playerController.PlayerRigidBody;

        if (inputDot > 0.5f) {
            // Freeze movement if player resists
            playerController.SetRepelDamping(true);
        } else {
            float force = settings.repelforceMagnitude;
            Vector3 repel = dirNormalized * force;
            rb.AddForceAtPosition(repel, rb.position, ForceMode.Impulse);
            playerController.SetRepelDamping(false);
        }
    }
}
