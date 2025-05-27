//// Author: Sadikur Rahman ////

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
    private bool wasInPolarityZone = false;
    private void Start()
    {
        polarityManager = DIContainer.Resolve<PolarityManager>();
        playerController = FindObjectOfType<PlayerController>();
    }

    private void Update() {
        CheckPolarityEffectZone();
    }

    private void FixedUpdate() {
        if (!isPolarityInEffect || playerController?.PlayerRigidBody == null) return;
        ApplyPoleEffect(playerController.PlayerRigidBody);
    }

    

    private void CheckPolarityEffectZone() {
        float dist = Vector3.Distance(transform.position, playerController.transform.position);
        isPolarityInEffect = dist <= settings.AreaDistance;

        // If player just left the polarity zone
        if (wasInPolarityZone && !isPolarityInEffect) {
            playerController.SetRepelDamping(false); // ✅ Reset damping smoothly
        }

        wasInPolarityZone = isPolarityInEffect;
    }

    private void ApplyPoleEffect(Rigidbody rb) {
        Vector3 playerPos = rb.transform.position;
        Vector3 polePos = transform.position;

        Vector3 direction = playerPos - polePos;
        direction.y = 0f;

        float distance = direction.magnitude;
        if (distance < 0.001f) return;

        Vector3 dirNormalized = direction / distance;
        bool samePolarity = polePolarity.polarity == polarityManager.CurrentPolarity.polarity;
        Vector3 inputDir = playerController.GetInputDirection();
        float dot = Vector3.Dot(inputDir, -dirNormalized); // >0 = player is pushing toward pole

        if (samePolarity) {
            // ✅ REPULSION — Freeze movement if player pushes in, repel otherwise
            if (dot > 0.5f) {
                // Player is resisting → freeze movement inside the field
                playerController.SetRepelDamping(true); 
            } else {
                // Repel with force
                float repelForce = settings.forceMagnitude * 0.5f;
                Vector3 repel = dirNormalized * repelForce;
                rb.AddForce(repel, ForceMode.Force);
                playerController.SetRepelDamping(false);
            }
        } else {
            // ✅ ATTRACTION — Pull until stuck, then stay stuck
            float attractForce = settings.forceMagnitude * 2f;

            if (!playerController.IsStuckToPole(transform)) {
                Vector3 pull = -dirNormalized * attractForce;
                rb.AddForce(pull, ForceMode.Force);
                
            } else {
                // Already stuck — keep gently pressing in so player doesn't slip
                Vector3 gentleSnap = -dirNormalized * 0.2f;
                rb.MovePosition(rb.position + gentleSnap * Time.fixedDeltaTime);
            }
            playerController.SetRepelDamping(false);
        }
    }
}
