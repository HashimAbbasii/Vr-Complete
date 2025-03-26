using UnityEngine;
using Photon.Pun;

public class ARPlayerManager : MonoBehaviourPunCallbacks
{
    public string vrCharacterPrefabName = "Y Bot"; // Must match the VR prefab name
    public ARCharacterTracker arTracker; // Assign in Inspector

    public override void OnJoinedRoom()
    {
        Debug.Log("AR Player Joined Room!");

        if (PhotonNetwork.IsMasterClient) return; // VR player is master, skip

        // Spawn the VR character in AR scene
        GameObject vrCharacter = PhotonNetwork.Instantiate(
            vrCharacterPrefabName,
            Vector3.zero,
            Quaternion.identity
        );

        // Assign the VR character's transform to the tracker
        if (arTracker != null)
        {
            arTracker.vrTarget = vrCharacter.transform;
            Debug.Log("Assigned VR Character to AR Tracker!");
        }
        else
        {
            Debug.LogError("ARCharacterTracker reference missing!");
        }
    }
}