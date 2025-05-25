//// Author: Sadikur Rahman ////

using UnityEngine;

public class MagnetPole : MonoBehaviour {
    public PolarityData polePolarity;
    public PoleSettings settings;

    private PolarityManager polarityManager;

    private void Start() {
        polarityManager = DIContainer.Resolve<PolarityManager>();
    }

    private void OnTriggerStay(Collider other) {
        if (!other.CompareTag("Player")) return;

        Rigidbody rb = other.attachedRigidbody;
        Vector3 dir = (other.transform.position - transform.position).normalized;

        bool samePolarity = polePolarity.polarity == polarityManager.CurrentPolarity.polarity;
        float force = settings.forceMagnitude * (samePolarity ? 0.5f : -1f);

        rb.AddForce(dir * force);
    }
}