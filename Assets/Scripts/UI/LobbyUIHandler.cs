using System.Collections;
using System.Collections.Generic;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyUIHandler : MonoBehaviour {

    [SerializeField]
    private GameObject roomPrefab;

    [SerializeField]
    private VerticalLayoutGroup verticalLayoutGroup;

    [SerializeField]
    private TextMeshProUGUI roomListState_Text;

    [SerializeField]
    private NetworkRunnerHandler networkRunnerHandler;

    [Header("Panels")]
    [SerializeField]
    private GameObject roomList_Panel;
    [SerializeField]
    private GameObject createRoom_Panel;
    [SerializeField]
    private GameObject transition_Panel;
    
    [Header("New room settings")]
    [SerializeField]
    private TMP_InputField roomName_InputField;

     void Awake() {
        OnLookingForGameSessions();
     }

     void Start() {

        networkRunnerHandler.OnJoinLobby();
     }

    private void HideAllPanels() {
          
        roomList_Panel.SetActive (false);
        createRoom_Panel.SetActive (false);
        transition_Panel.SetActive (false);
    }

    public void ClearRoomList() {
        
        foreach (Transform room in verticalLayoutGroup.transform) {
            Destroy (room.gameObject);
        }

        roomListState_Text.gameObject.SetActive (false);
    }

    public void AddToRoomList (SessionInfo sessionInfo) {

        RoomInfo roomInfo = Instantiate (roomPrefab, verticalLayoutGroup.transform).GetComponent<RoomInfo>();
    
        roomInfo.SetInformation (sessionInfo);

        roomInfo.OnJoinRoom += AddedRoom_OnJoinSession;

    }

    private void AddedRoom_OnJoinSession(SessionInfo sessionInfo) {

        networkRunnerHandler.JoinGame(sessionInfo);

        OnJoiningRoom();
    }

    public void OnLookingForGameSessions() {

        ClearRoomList();

        roomListState_Text.text = "Looking for game rooms";
        roomListState_Text.gameObject.SetActive(true);
    }

    public void OnNoSessionsFound() {

        ClearRoomList();

        roomListState_Text.text = "No game room found";
        roomListState_Text.gameObject.SetActive(true);
    }

    public void OnBackToMenuClicked() {
        SceneManager.LoadScene("Start");
    }

    public void OnCreatNewRoomClicked() {
        
        HideAllPanels();

        createRoom_Panel.SetActive (true);
    }

    public void OnCreatRoomClicked() {

        networkRunnerHandler.CreateGame(roomName_InputField.text, "Gaming");

        HideAllPanels();

        transition_Panel.gameObject.SetActive(true);
    }

    public void OnJoiningRoom() {

        HideAllPanels();

        transition_Panel.gameObject.SetActive(true);
    }
    
}
