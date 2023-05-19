using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;
using TMPro;

public class NetworkPlayer : NetworkBehaviour {

    public static NetworkPlayer Local {get; set;}

    public Transform playerModel;

    [Networked (OnChanged = nameof(OnNickNameChanged))]
    public NetworkString<_16> nickName {set; get;}

    [SerializeField]
    private TextMeshProUGUI playerNickNameTMP;

    [SerializeField]
    private FirstPerosonCamera firstPerosonCamera;

    [SerializeField]
    private GameObject localUI;

    // Remote Client Token Hash
    [Networked]
    public int token {get; set;}

    public override void Spawned() {
        
        // 生成local player時
        if (Object.HasInputAuthority) {

            Local = this;

            //Sets the layer of the local players model
            Utils.SetRenderLayerInChildren(playerModel, LayerMask.NameToLayer("LocalPlayerModel"));

            //Enable 1 audio listner
            // AudioListener audioListener = GetComponentInChildren<AudioListener>(true);
            // audioListener.enabled = true;

            // Enable the local camera
            firstPerosonCamera.localCamera.enabled = true;

            // Detach the local camera
            firstPerosonCamera.localCamera.transform.parent = null;

            // Enable the local UI
            localUI.SetActive (true);

            RPC_SetNickName(GameManager.instance.playerNickName);

            print ("Spawned local player");
        }

        // 生成remote player時
        else {

            //Disable the local camera for remote players
            firstPerosonCamera.localCamera.enabled = false;

            //Disable UI for remote player
            localUI.SetActive(false);

            //Only 1 audio listner is allowed in the scene so disable remote players audio listner
            // AudioListener audioListener = GetComponentInChildren<AudioListener>();
            // audioListener.enabled = false;

            print ("Spawned remote player");
        }

        // Make it easier to tell which player is which.
        transform.name = $"P_{Object.Id}";
    }

    static void OnNickNameChanged(Changed<NetworkPlayer> changed) {

        changed.Behaviour.OnNickNameChanged();

    }

    private void OnNickNameChanged() {

        Debug.Log($"Nickname changed to {nickName} for player {gameObject.name}");

        playerNickNameTMP.text = nickName.ToString();

    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    public void RPC_SetNickName(string nickName, RpcInfo info = default) {

        Debug.Log($"[RPC] SetNickName {nickName}");
        this.nickName = nickName;

        // if(!isPublicJoinMessageSent)
        // {
        //     networkInGameMessages.SendInGameRPCMessage(nickName, "joined");

        //     isPublicJoinMessageSent = true;
        // }
    }
    
    void OnDestroy() {
        if (firstPerosonCamera != null) {
            Destroy (firstPerosonCamera.gameObject);
        }
    }
}
