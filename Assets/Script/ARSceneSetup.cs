using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class ARSceneSetup : MonoBehaviourPunCallbacks
{
    public ARCharacterTracker arTracker; // Drag your tracking camera/object here

    void Start()
    {
        // Connect to Photon network
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        // Join or create room
        PhotonNetwork.JoinOrCreateRoom(
            "VR_AR_Room",
            new RoomOptions { MaxPlayers = 2 },
            TypedLobby.Default
        );
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("AR Scene Joined Room");

        // Find VR character in scene
        GameObject vrCharacter = GameObject.FindGameObjectWithTag("VRPlayer");

        if (vrCharacter != null)
        {
            arTracker.SetVRCharacterTransform(vrCharacter.transform);
            Debug.Log("AR Tracking VR Character");
        }
        else
        {
            Debug.LogWarning("No VR Character Found!");
        }
    }
}