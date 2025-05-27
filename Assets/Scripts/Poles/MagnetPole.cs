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
    private bool isAppliedRepelEffect;

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

        if (samePolarity)
        {
            //  REPULSION — Freeze movement if player pushes in, repel otherwise
            ApplyRepelEffect(rb,dot, dirNormalized);
        } 
        else 
        {
            //  ATTRACTION — Pull until stuck, then stay stuck
            ApplyAttractEffect(dirNormalized,rb);
        }
    }
    private void ApplyAttractEffect(Vector3 dirNormalized, Rigidbody rb)
    {
        float attractForce = settings.attarctforceMagnitude;
        if (!playerController.IsStuckToPole(transform)) {
            Vector3 pull = -dirNormalized * attractForce;
            rb.AddForceAtPosition(pull,this.transform.position, ForceMode.Force);
                
        } else {
            // Already stuck — keep gently pressing in so player doesn't slip
            Vector3 gentleSnap = -dirNormalized ;
            rb.MovePosition(rb.position + gentleSnap * Time.fixedDeltaTime);
        }
        playerController.SetRepelDamping(false);
    }

    private void ApplyRepelEffect(Rigidbody rb, float dot,Vector3 dirNormalized)
    {
        if (dot > 0.5f) {
            // Player is resisting → freeze movement inside the field
            playerController.SetRepelDamping(true); 
        }
        else {
            // Repel with force
            float repelForce = settings.repelforceMagnitude;
            Vector3 repel = dirNormalized * repelForce ;
            rb.AddForceAtPosition(repel,rb.position, ForceMode.Impulse);
            playerController.SetRepelDamping(false);
        }
    }
}
