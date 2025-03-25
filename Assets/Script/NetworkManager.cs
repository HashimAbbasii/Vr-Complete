using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [Tooltip("Auto-join room on startup?")]
    public bool autoJoinRoom = true;

    void Start()
    {
        Debug.Log("Connecting to Photon...");
        PhotonNetwork.ConnectUsingSettings();
    }

    // Called when connected to Photon Master Server
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Master Server!");

        if (autoJoinRoom)
        {
            RoomOptions roomOptions = new RoomOptions()
            {
                MaxPlayers = 2, // VR + AR clients
                IsVisible = true
            };

            PhotonNetwork.JoinOrCreateRoom("VRAR_Room", roomOptions, TypedLobby.Default);
        }
    }

    // Called when successfully joined a room
    public override void OnJoinedRoom()
    {
        Debug.Log($"Joined Room: {PhotonNetwork.CurrentRoom.Name} | " +
                $"Players: {PhotonNetwork.CurrentRoom.PlayerCount}");
    }

    // Called if joining a room fails
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"Failed to join room: {message} (Error: {returnCode})");
    }

    // Called when disconnected
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning($"Disconnected: {cause}. Reconnecting...");
        PhotonNetwork.Reconnect();
    }
}