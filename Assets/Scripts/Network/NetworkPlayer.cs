using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;
using TMPro;

public class NetworkPlayer : NetworkBehaviour {

    public static NetworkPlayer Local {get; set;}

    public Transform playerModel;

    [SerializeField]
    private TextMeshProUGUI playerNickNameTMP;

    
    [Networked (OnChanged = nameof(OnNickNameChanged))]
    public NetworkString<_16> nickName {set; get;}

    public override void Spawned() {
        
        // 生成local player時
        if (Object.HasInputAuthority) {

            print ("Spawned local player");

            Local = this;

            //Sets the layer of the local players model
            Utils.SetRenderLayerInChildren(playerModel, LayerMask.NameToLayer("LocalPlayerModel"));

            RPC_SetNickName(PlayerPrefs.GetString("PlayerNickName"));

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
}
