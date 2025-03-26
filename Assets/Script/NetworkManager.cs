using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [Header("Settings")]
    [Tooltip("Automatically join room on connection")]
    public bool autoJoinRoom = true;
    [Tooltip("Room name for VR/AR sync")]
    public string roomName = "VRAR_Room";
    [Tooltip("Max VR+AR players allowed")]
    public byte maxPlayers = 2;

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
        Debug.Log("Connecting to Photon...");

        // Ensure you're not already connected
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            OnConnectedToMaster();
        }
    }

    public override void OnConnectedToMaster()
    {
        isConnecting = false;
        Debug.Log("Connected to Photon Master Server");

        if (autoJoinRoom)
        {
            // Use JoinRandomRoom with fallback
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"Join Random Failed: {message}. Creating new room...");

        // Create room with specific options
        PhotonNetwork.CreateRoom(roomName, new RoomOptions
        {
            MaxPlayers = maxPlayers,
            IsVisible = true,
            PublishUserId = true,
            EmptyRoomTtl = 0
        });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"Joined Room: {PhotonNetwork.CurrentRoom.Name} | " +
                 $"Players: {PhotonNetwork.CurrentRoom.PlayerCount}/" +
                 $"{PhotonNetwork.CurrentRoom.MaxPlayers}");

        // Determine client role more explicitly
        DetermineClientRole();
    }

    void DetermineClientRole()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            // First player is always VR
            Debug.Log("This is the VR Client (First Player)");
        }
        else if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            // Second player is AR
            Debug.Log("This is the AR Client (Second Player)");
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"Join Room Failed: {message}");
        isConnecting = false;

        // Optional: Retry connection
        Invoke(nameof(ConnectToPhoton), 3f);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        isConnecting = false;
        Debug.LogWarning($"Disconnected: {cause}. Reconnecting...");
        ConnectToPhoton();
    }
}