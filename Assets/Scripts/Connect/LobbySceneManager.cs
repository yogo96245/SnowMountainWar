using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class LobbySceneManager : MonoBehaviourPunCallbacks {
    void Start() {
        if (PhotonNetwork.IsConnected) {
            PhotonNetwork.JoinLobby();      
        }
        else {
            SceneManager.LoadScene ("Start");
        }  
    }
    public override void OnJoinedLobby() {
        print ("Lobby Joined");
    }
}
