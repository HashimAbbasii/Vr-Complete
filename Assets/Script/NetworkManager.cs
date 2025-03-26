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

    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true; // Critical for multi-scene sync
        ConnectToPhoton();
    }

    void ConnectToPhoton()
    {
        Debug.Log("Connecting to Photon...");
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = "1.0"; // Set version to prevent mismatches
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Master Server");

        if (autoJoinRoom)
        {
            var roomOptions = new RoomOptions
            {
                MaxPlayers = maxPlayers,
                IsVisible = true,
                PublishUserId = true, // Helps with device identification
                EmptyRoomTtl = 0 // Room destroys when empty
            };

            PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"Joined Room: {PhotonNetwork.CurrentRoom.Name} | " +
                 $"Players: {PhotonNetwork.CurrentRoom.PlayerCount}/" +
                 $"{PhotonNetwork.CurrentRoom.MaxPlayers}");

        // Critical for VR/AR role assignment
        Debug.Log($"I am {(PhotonNetwork.IsMasterClient ? "VR" : "AR")} client");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"Player joined: {newPlayer.UserId} (Actor #{newPlayer.ActorNumber})");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"Join failed: {message}. Creating new room...");
        PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = maxPlayers });
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning($"Disconnected: {cause}. Reconnecting in 3s...");
        Invoke(nameof(ConnectToPhoton), 3f); // Safer delayed reconnect
    }
}