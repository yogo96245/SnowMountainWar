using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;

public class NetworkPlayer : NetworkBehaviour {

    public static NetworkPlayer Local {get; set;}

    public Transform playerModel;

    public override void Spawned() {
        
        // 生成local player時
        if (Object.HasInputAuthority) {

            print ("Spawned local player");

            Local = this;   

            //Sets the layer of the local players model
            Utils.SetRenderLayerInChildren(playerModel, LayerMask.NameToLayer("LocalPlayerModel"));

            
        }
        // 生成remote player時
        else {

            print ("Spawned remote player");

            Camera remoteCamera = GetComponentInChildren<Camera>();
            remoteCamera.enabled = false;

            Canvas remoteCanvas  = GetComponentInChildren<Canvas>();
            remoteCanvas.gameObject.SetActive (false);

        }

        // Make it easier to tell which player is which.
        transform.name = $"P_{Object.Id}";
    }
}
