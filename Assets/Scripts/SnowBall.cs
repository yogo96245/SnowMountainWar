using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class SnowBall : NetworkBehaviour {

    [SerializeField]
    private float shootSpeed =  5.0f;

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
}
