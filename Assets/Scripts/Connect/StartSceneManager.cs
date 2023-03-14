using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviourPunCallbacks {
    void OnMouseDown() {
        PhotonNetwork.ConnectUsingSettings();
        print ("ClickStart");
    }

    public override void OnConnectedToMaster() {
        print ("Connected!");
        SceneManager.LoadScene ("Lobby");
    }
}
