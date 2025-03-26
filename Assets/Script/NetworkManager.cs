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
        PhotonNetwork.AutomaticallySyncScene = true;  // Ensure all clients load the same scene
        PhotonNetwork.ConnectUsingSettings();
    }

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

    public override void OnJoinedRoom()
    {
        Debug.Log($"Joined Room: {PhotonNetwork.CurrentRoom.Name} | Players: {PhotonNetwork.CurrentRoom.PlayerCount}");

        // Debugging - Check who is in the room
        foreach (var player in PhotonNetwork.PlayerList)
        {
            Debug.Log($"Player in Room: {player.ActorNumber} - IsMasterClient: {player.IsMasterClient}");
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"Failed to join room: {message} (Error: {returnCode})");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning($"Disconnected: {cause}. Reconnecting...");
        PhotonNetwork.Reconnect();
    }
}
