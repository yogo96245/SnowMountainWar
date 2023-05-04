using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class SnowBall : NetworkBehaviour {

    [SerializeField]
    private float shootSpeed =  5.0f;

    [SerializeField]
    private byte demage =  10;

    [Networked]
    private TickTimer life {set; get;}

    public override void Spawned() {
        life = TickTimer.CreateFromSeconds (Runner, 5.0f);    
    }

    public override void FixedUpdateNetwork() {
        if (life.Expired (Runner)) {
            Runner.Despawn (Object);
        }
        else {
            transform.position += shootSpeed * transform.forward * Runner.DeltaTime;    
        }
    }

    private void OnTriggerEnter (Collider other) {

        var networkObject = other.GetComponent<NetworkObject>();

        if (other.CompareTag ("Player") && (Object.InputAuthority != networkObject.InputAuthority)) {

            var playerState = other.GetComponent<PlayerState>();
            playerState.TakeDemage (demage);

            Runner.Despawn (Object);
        }
    }
}
