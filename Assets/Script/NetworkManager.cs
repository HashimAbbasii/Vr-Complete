using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ARSceneSetup : MonoBehaviourPunCallbacks
{
    [Header("Network Settings")]
    public string roomName = "VRAR_Room";
    public byte maxPlayers = 2;

    private bool isConnecting = false;

    void Start()
    {
        // Ensure scene sync is enabled
        PhotonNetwork.AutomaticallySyncScene = true;

        // Connect to Photon
        ConnectToPhoton();
    }

    void ConnectToPhoton()
    {
        if (isConnecting) return;

        isConnecting = true;
        Debug.Log("AR Scene: Attempting to connect to Photon...");

        // Reset Photon state if needed
        if (PhotonNetwork.IsMessageQueueRunning)
        {
            PhotonNetwork.Disconnect();
        }

        // Explicitly connect
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("AR Scene: Connected to Master Server");
        isConnecting = false;

        // Attempt to join or create room
        TryJoinRoom();
    }

    void TryJoinRoom()
    {
        Debug.Log("AR Scene: Attempting to join room...");

        // Ensure we're in the correct network state
        if (PhotonNetwork.IsConnectedAndReady)
        {
            RoomOptions roomOptions = new RoomOptions
            {
                MaxPlayers = maxPlayers,
                IsVisible = true,
                PublishUserId = true,
                EmptyRoomTtl = 0
            };

            // Try to join or create room
            PhotonNetwork.JoinOrCreateRoom(
                roomName,
                roomOptions,
                TypedLobby.Default
            );
        }
        else
        {
            Debug.LogWarning("AR Scene: Not ready to join room. Reconnecting...");
            Invoke(nameof(ConnectToPhoton), 2f);
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"AR Scene: Joined Room {PhotonNetwork.CurrentRoom.Name}");
        Debug.Log($"Players in Room: {PhotonNetwork.CurrentRoom.PlayerCount}");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"AR Scene: Join Room Failed - {message} (Code: {returnCode})");

        // Retry joining
        Invoke(nameof(TryJoinRoom), 2f);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"AR Scene: Create Room Failed - {message} (Code: {returnCode})");

        // Retry connecting
        Invoke(nameof(ConnectToPhoton), 2f);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning($"AR Scene: Disconnected - {cause}");
        isConnecting = false;

        // Automatically reconnect
        Invoke(nameof(ConnectToPhoton), 2f);
    }
}