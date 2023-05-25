using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Fusion;


public class RoomInfo : MonoBehaviour {

    [SerializeField]
    private TextMeshProUGUI roomName_Text;

    [SerializeField]
    private TextMeshProUGUI playerCount_Text;

    [SerializeField]
    private Button joinRoom_Button;

    private SessionInfo sessionInfo;

    //Events
    public event Action<SessionInfo> OnJoinRoom;

    public void SetInformation (SessionInfo sessionInfo) {
        
        this.sessionInfo = sessionInfo;

        roomName_Text.text = sessionInfo.Name;
        playerCount_Text.text = $"{sessionInfo.PlayerCount.ToString()}/{sessionInfo.MaxPlayers.ToString()}";

        bool canJoin = true;

        if (sessionInfo.PlayerCount >= sessionInfo.MaxPlayers) {
            canJoin = false;
        }

        joinRoom_Button.gameObject.SetActive(canJoin);
    }

    public void OnJoinRoomClick() {
        //Invoke the join session event
        OnJoinRoom?.Invoke(sessionInfo);
    }
}
