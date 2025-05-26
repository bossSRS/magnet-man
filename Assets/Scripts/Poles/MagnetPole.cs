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
    public bool isAttachWithPlayer;

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
        float forceScale = samePolarity ? 1f : -5f;
        Vector3 magneticForce = horizontalDir * settings.forceMagnitude * forceScale;
        var forceVector3 = new Vector3(magneticForce.x, horizontalDir.y, magneticForce.z);
        // Preserve Y (vertical) velocity for jumping
        if (samePolarity)
        {
            rb.linearVelocity = forceVector3;
        }
        else
        {
            if(!isAttachWithPlayer)rb.AddForce(forceVector3,ForceMode.Acceleration);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if(!other.gameObject.CompareTag("Player")) return;
        isAttachWithPlayer = true;
    }

    private void OnCollisionExit(Collision other)
    {
        if(!other.gameObject.CompareTag("Player")) return;
        isAttachWithPlayer = false;
    }
}