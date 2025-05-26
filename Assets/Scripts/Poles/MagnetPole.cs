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
    private void ApplyPoleEffect(Rigidbody playerRigidBody)
    {
        if(isNeutralInEffefct) isNeutralInEffefct = false;
        Rigidbody rb = playerRigidBody;
        Vector3 dir = (playerRigidBody.transform.position - transform.position).normalized;

        bool samePolarity = polePolarity.polarity == polarityManager.CurrentPolarity.polarity;
        float force = settings.forceMagnitude * (samePolarity ? 1 : -4f);

        //rb.AddForce(dir * force, ForceMode.Acceleration);
        rb.linearVelocity = dir * force;
    }
}