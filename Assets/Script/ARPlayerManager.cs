using UnityEngine;
using Photon.Pun;

public class ARPlayerManager : MonoBehaviourPunCallbacks
{
    public string vrCharacterPrefabName = "Y Bot";
    public ARCharacterTracker arTracker; // Assign in inspector

    public override void OnJoinedRoom()
    {
        Debug.Log("AR Player Joined Room!");

        if (PhotonNetwork.IsMasterClient) return;

        GameObject vrCharacter = PhotonNetwork.Instantiate(vrCharacterPrefabName, Vector3.zero, Quaternion.identity);
        Debug.Log("Spawned VR Character in AR Scene: " + vrCharacter.name);

        if (arTracker != null)
        {
            arTracker.vrTarget = vrCharacter.transform;
            Debug.Log("Assigned VR Character to AR Tracker!");
        }
        else
        {
            Debug.LogError("ARCharacterTracker reference is missing!");
        }
    }
}