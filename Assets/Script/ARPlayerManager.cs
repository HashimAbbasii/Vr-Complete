using UnityEngine;
using Photon.Pun;

public class ARPlayerManager : MonoBehaviourPunCallbacks
{
    private ARCharacterTracker arTracker;

    void Start()
    {
        arTracker = FindObjectOfType<ARCharacterTracker>();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("AR Player Joined Room!");

        GameObject vrPlayer = FindVRPlayer();
        if (vrPlayer != null)
        {
            arTracker.vrTarget = vrPlayer.transform;
            Debug.Log("Tracking VR Player in AR Scene: " + vrPlayer.name);
        }
        else
        {
            Debug.LogError("No VR Player Found!");
        }
    }

    private GameObject FindVRPlayer()
    {
        foreach (var player in PhotonNetwork.PlayerListOthers)
        {
            if (player.IsMasterClient) // VR Player is always the Master Client
            {
                return PhotonView.Find(player.ActorNumber)?.gameObject;
            }
        }
        return null;
    }
}
