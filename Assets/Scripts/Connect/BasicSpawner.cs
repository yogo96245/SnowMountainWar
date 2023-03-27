using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using UnityEngine.SceneManagement;

public class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks {
    
    private NetworkRunner networkRunner = null;

    [SerializeField]
    private NetworkPrefabRef playerPrefab;

    private Dictionary<PlayerRef, NetworkObject> playerList = new Dictionary<PlayerRef, NetworkObject>();

    void Start() {
        StartGame (GameMode.AutoHostOrClient);
    }

    async void StartGame(GameMode mode) {
    
        // Create the Fusion runner and let it know that we will be providing user input
        networkRunner = gameObject.AddComponent<NetworkRunner>();
        networkRunner.ProvideInput = true;

        // Start or join (depends on gamemode) a session with a specific name
        await networkRunner.StartGame(new StartGameArgs() {
            GameMode = mode,
            SessionName = "FirstRoom",
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) {
        print ("player joned");
         Vector3 spawnPosition = new Vector3 (-756f, 10.2f, -529);
        NetworkObject networkPlayerObject = runner.Spawn(playerPrefab, spawnPosition, Quaternion.identity, player);
        // Keep track of the player avatars so we can remove it when they disconnect
        playerList.Add(player, networkPlayerObject);
    }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) {
        if (playerList.TryGetValue(player, out NetworkObject networkObject)) {
            runner.Despawn(networkObject);
            playerList.Remove(player);
        }
    }
    public void OnInput(NetworkRunner runner, NetworkInput input) {
        var data = new NetworkInputData();

        float horizontal  = Input.GetAxis ("Horizontal");
        float vertical = Input.GetAxis ("Vertical");
        data.movementInput.Set(horizontal, 0f, vertical);

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y") * -1;
        data.rotateInput.Set (mouseY, mouseX);

        // 傳送 input 資料給 server
        input.Set(data);

        // if (Input.GetKey(KeyCode.W))
        //   data.movementInput += Vector3.forward;

        // if (Input.GetKey(KeyCode.S))
        //   data.movementInput += Vector3.back;

        // if (Input.GetKey(KeyCode.A))
        //   data.movementInput += Vector3.left;

        // if (Input.GetKey(KeyCode.D))
        //   data.movementInput += Vector3.right;
    }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) {
        print ("player connected server");
    }
    public void OnDisconnectedFromServer(NetworkRunner runner) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) {}
}
