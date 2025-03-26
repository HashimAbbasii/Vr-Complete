using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [Header("Network Settings")]
    public string roomName = "VRAR_Room";
    public byte maxPlayers = 2;
    public bool isVRScene = false; // Manually set in inspector

    private bool isConnecting = false;

    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        ConnectToPhoton();
    }

    void ConnectToPhoton()
    {
        if (isConnecting) return;

        isConnecting = true;
        Debug.Log($"{(isVRScene ? "VR" : "AR")} Scene: Attempting to connect to Photon...");

        if (PhotonNetwork.IsMessageQueueRunning)
        {
            PhotonNetwork.Disconnect();
        }

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log($"{(isVRScene ? "VR" : "AR")} Scene: Connected to Master Server");
        isConnecting = false;
        TryJoinRoom();
    }

    void TryJoinRoom()
    {
        Debug.Log($"{(isVRScene ? "VR" : "AR")} Scene: Attempting to join room...");

        if (PhotonNetwork.IsConnectedAndReady)
        {
            RoomOptions roomOptions = new RoomOptions
            {
                MaxPlayers = maxPlayers,
                IsVisible = true,
                PublishUserId = true,
                EmptyRoomTtl = 0
            };

            // VR scene tries to create room, AR scene tries to join
            if (isVRScene)
            {
                PhotonNetwork.CreateRoom(
                    roomName,
                    roomOptions,
                    TypedLobby.Default
                );
            }
            else
            {
                PhotonNetwork.JoinRoom(roomName);
            }
        }
        else
        {
            Debug.LogWarning($"{(isVRScene ? "VR" : "AR")} Scene: Not ready to join room. Reconnecting...");
            Invoke(nameof(ConnectToPhoton), 2f);
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"{(isVRScene ? "VR" : "AR")} Scene: Joined Room {PhotonNetwork.CurrentRoom.Name}");
        Debug.Log($"Players in Room: {PhotonNetwork.CurrentRoom.PlayerCount}");

        // Specific actions based on scene type
        if (isVRScene)
        {
            HandleVRSceneJoined();
        }
        else
        {
            HandleARSceneJoined();
        }
    }

    void HandleVRSceneJoined()
    {
        // VR-specific room joining logic
        Debug.Log("VR Scene: Waiting for AR client to join...");
    }

    void HandleARSceneJoined()
    {
        // AR-specific room joining logic
        Debug.Log("AR Scene: Connected to VR session");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"{(isVRScene ? "VR" : "AR")} Scene: Join Room Failed - {message} (Code: {returnCode})");

        if (!isVRScene)
        {
            // If AR scene fails to join, wait and retry
            Invoke(nameof(TryJoinRoom), 2f);
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"VR Scene: Create Room Failed - {message} (Code: {returnCode})");
        Invoke(nameof(ConnectToPhoton), 2f);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning($"{(isVRScene ? "VR" : "AR")} Scene: Disconnected - {cause}");
        isConnecting = false;
        Invoke(nameof(ConnectToPhoton), 2f);
    }
}