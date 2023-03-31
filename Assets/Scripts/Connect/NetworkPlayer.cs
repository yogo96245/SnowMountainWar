using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class NetworkPlayer : NetworkBehaviour {

    public static NetworkPlayer Local {get; set;}

    public override void Spawned() {
        if (Object.HasInputAuthority) {
            Local = this;   

            print ("Spawned local player");
        }
        else {
            // 關閉remote player的Camera
            Camera localCamera = GetComponentInChildren<Camera>();
            localCamera.enabled = false;

            print ("Spawned remote player");
        }
    }
}
