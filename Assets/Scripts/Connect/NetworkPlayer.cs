using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class NetworkPlayer : NetworkBehaviour {

    public static NetworkPlayer Local {get; set;}

    public Transform playerModel;

    public override void Spawned() {
        if (Object.HasInputAuthority) {
            Local = this;   

            //Sets the layer of the local players model
            Utils.SetRenderLayerInChildren(playerModel, LayerMask.NameToLayer("LocalPlayerModel"));

            print ("Spawned local player");
        }
        // 關閉remote player的Camera
        else {
            Camera localCamera = GetComponentInChildren<Camera>();
            localCamera.enabled = false;

            print ("Spawned remote player");
        }

        // Make it easier to tell which player is which.
        transform.name = $"P_{Object.Id}";
    }
}
