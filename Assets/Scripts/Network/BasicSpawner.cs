using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using UnityEngine.SceneManagement;

public class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks {

    [SerializeField]
    private NetworkPlayer playerPrefab;

    private PlayerInput playerInput;

    private Dictionary<int, NetworkPlayer> mapTokenIDWithNetworkPlayer;

    private LobbyUIHandler lobbyUIHandler;

    void Awake() {
        mapTokenIDWithNetworkPlayer = new Dictionary<int, NetworkPlayer>();
        lobbyUIHandler = FindObjectOfType<LobbyUIHandler>();
    }

    void Start() {
    }

    private int GetPlayerToken (NetworkRunner runner, PlayerRef player) {

        if (runner.LocalPlayer == player) {

            // Just use the local Player Connection Token
            return ConnectionTokenUtils.HashToken(GameManager.instance.GetConnectionToken());
        }

        else {

            // Get the Connection Token stored when the Client connects to this Host
            var token = runner.GetPlayerConnectionToken(player);

            if (token != null) {
                return ConnectionTokenUtils.HashToken(token);
            }

            Debug.LogError($"GetPlayerToken returned invalid token");

            // invaled token
            return 0;
        }
        
    }

    public void SetConnectionTokenMapping(int token, NetworkPlayer networkPlayer) {
        mapTokenIDWithNetworkPlayer.Add(token, networkPlayer);
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) {

        if (runner.IsServer) {

            int playerToken = GetPlayerToken(runner, player);

            if (mapTokenIDWithNetworkPlayer.TryGetValue (playerToken, out NetworkPlayer networkPlayer)) {
                
                Debug.Log($"Found old connection token for token {playerToken}. Assigning controlls to that player");

                networkPlayer.GetComponent<NetworkObject>().AssignInputAuthority(player);

                networkPlayer.Spawned();
            }

            else {
                
                Debug.Log($"Spawning new player for connection token {playerToken}");

                Vector3 spawnPosition = new Vector3 (-756f, 10.2f, -529);
                NetworkPlayer spawnedNetworkPlayer = runner.Spawn(playerPrefab, spawnPosition, Quaternion.identity, player);

                spawnedNetworkPlayer.token = playerToken;

                //Store the mapping between playerToken and the spawned network player
                mapTokenIDWithNetworkPlayer[playerToken] = spawnedNetworkPlayer;
            }
            
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) {

        // if (runner.IsServer) {

        //     Debug.Log ("player left");

        //     Debug.Log (player);

        //     // Error: miss player token
        //     var token = runner.GetPlayerConnectionToken(player);

        //     Debug.Log ($"token: {token}");

        //     int hashToken = ConnectionTokenUtils.HashToken (token);

        //     Debug.Log ($"hashToken: {hashToken}");

        //     if (mapTokenIDWithNetworkPlayer.TryGetValue(hashToken, out NetworkPlayer networkObject)) {

        //         Debug.Log ($"player left token:{hashToken}");

        //         // mapTokenIDWithNetworkPlayer.Remove (hashToken);

        //         Debug.Log ($"currnt mapTokenIDWithNetworkPlayer: {mapTokenIDWithNetworkPlayer.Count}");
        //     }

        //     else {
        //         Debug.Log ("Not found player token when player left");
        //     }
        // }
    }

    public void OnInput(NetworkRunner runner, NetworkInput input) {
        if (playerInput == null && NetworkPlayer.Local != null) {
            playerInput = NetworkPlayer.Local.GetComponent<PlayerInput>();
        }

        if (playerInput != null) {
            input.Set(playerInput.GetNetworkInput());
        }
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) {
        Debug.Log ("OnShutdown");
     }

    public void OnConnectedToServer(NetworkRunner runner) {
        Debug.Log ("player connected server");
    }

    public void OnDisconnectedFromServer(NetworkRunner runner) { 
        Debug.Log ("OnDisconnectedFromServer");
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { 

        //Only update the list of sessions when the session list UI handler is active
        if (lobbyUIHandler == null) {
            Debug.Log ("not found lobbyUIHandler");
            return;
        }
            
        if (sessionList.Count == 0) {

            Debug.Log("Joined lobby no sessions found");

            lobbyUIHandler.OnNoSessionsFound();
        }
        else {

            Debug.Log ($"sessionList.Count: {sessionList.Count}");

            lobbyUIHandler.ClearRoomList();

            foreach (SessionInfo sessionInfo in sessionList) {

                lobbyUIHandler.AddToRoomList(sessionInfo);

                Debug.Log($"Found session {sessionInfo.Name} playerCount {sessionInfo.PlayerCount}");
            }
        }
    }
    
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
    
    public void OnSceneLoadDone(NetworkRunner runner) { }
    
    public void OnSceneLoadStart(NetworkRunner runner) { }

    public async void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) {

        Debug.Log ("OnHostMigration");

        await runner.Shutdown (shutdownReason: ShutdownReason.HostMigration);
        
        FindObjectOfType<NetworkRunnerHandler>().StartHostMigration(hostMigrationToken);
    } 

    public void OnHostMigrationCleanUp() {

        Debug.Log("Spawner OnHostMigrationCleanUp started");

        foreach (KeyValuePair<int, NetworkPlayer> entry in mapTokenIDWithNetworkPlayer) {

            NetworkObject networkObjectInDictionary = entry.Value.GetComponent<NetworkObject>();

            if (networkObjectInDictionary.InputAuthority.IsNone) {

                Debug.Log($"Found player that has not reconnected. Despawning {entry.Value.nickName}");

                networkObjectInDictionary.Runner.Despawn(networkObjectInDictionary);

                // Debug.Log($"mapTokenIDWithNetworkPlaye remove token {entry.Key} player");

                // mapTokenIDWithNetworkPlayer.Remove (entry.Key);

                // Debug.Log ($"currnt mapTokenIDWithNetworkPlayer: {mapTokenIDWithNetworkPlayer.Count}");
            }
        }

        Debug.Log("Spawner OnHostMigrationCleanUp completed");
    }
}
