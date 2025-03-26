using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public bool autoJoinRoom = true;

    void Start()
    {
        Debug.Log("Connecting to Photon...");
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
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"Player joined: {newPlayer.ActorNumber} | Total: {PhotonNetwork.CurrentRoom.PlayerCount}");
    }
}
