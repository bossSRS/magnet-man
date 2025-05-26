//// Author: Sadikur Rahman ////

using System;
using UnityEngine;

public class MagnetPole : MonoBehaviour {
    public PolarityData polePolarity;
    public PoleSettings settings;

    private PolarityManager polarityManager;
    private Rigidbody playerRigidbody;
    public bool isPolarityInEffefct;
    public bool isNeutralInEffefct;
    private void Start() 
    {
        polarityManager = DIContainer.Resolve<PolarityManager>();
        playerRigidbody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
    }
    public void Update()
    {
        CheckForPolarityEffect();
    }
    public void FixedUpdate()
    {
        if (!playerRigidbody) return;
        
        if(isPolarityInEffefct)
            ApplyPoleEffect(playerRigidbody);
        else if(!isPolarityInEffefct && !isNeutralInEffefct)
            ApplyNeutralEffect(playerRigidbody);
            
    }

    private void ApplyNeutralEffect(Rigidbody playerRigidbody)
    {
        playerRigidbody.linearVelocity = Vector3.zero;
        isNeutralInEffefct = true;
    }

    private void CheckForPolarityEffect()
    {
        isPolarityInEffefct = (Vector3.Distance(transform.position, playerRigidbody.transform.position) <= settings.AreaDistance);
    }
    private void ApplyPoleEffect(Rigidbody rb) {
        if (isNeutralInEffefct)
            isNeutralInEffefct = false;

        Vector3 playerPos = rb.transform.position;
        Vector3 polePos = transform.position;

        // Flatten direction to horizontal plane
        Vector3 horizontalDir = (playerPos - polePos);
        horizontalDir.y = 0f;
        horizontalDir.Normalize();

        // Polarity logic: repel = +1, attract = -2 (stronger pull)
        bool samePolarity = polePolarity.polarity == polarityManager.CurrentPolarity.polarity;
        float forceScale = samePolarity ? 1f : -2f;
        Vector3 magneticForce = horizontalDir * settings.forceMagnitude * forceScale;

        // Preserve Y (vertical) velocity for jumping
        rb.linearVelocity = new Vector3(magneticForce.x, rb.linearVelocity.y, magneticForce.z);
    }
}