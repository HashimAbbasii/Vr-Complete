using UnityEngine;
using Photon.Pun;

public class ARPlayerManager : MonoBehaviourPunCallbacks
{
    public string vrCharacterPrefabName = "Y Bot"; // Ensure this matches the exact name in Resources

    public override void OnJoinedRoom()
    {
        Debug.Log("AR Player Joined Room!");

        if (PhotonNetwork.IsMasterClient) return; // VR Player is master, AR shouldn't instantiate

        if (!string.IsNullOrEmpty(vrCharacterPrefabName))
        {
            GameObject vrCharacter = PhotonNetwork.Instantiate(vrCharacterPrefabName, Vector3.zero, Quaternion.identity);
            Debug.Log("Spawned VR Character in AR Scene: " + vrCharacter.name);

            // Find ARCharacterTracker in the scene
            ARCharacterTracker arTracker = FindObjectOfType<ARCharacterTracker>();
            if (arTracker != null)
            {
                arTracker.vrTarget = vrCharacter.transform; // ✅ Assign VR Character as the Target
                Debug.Log("Assigned VR Character to AR Tracker!");
            }
            else
            {
                Debug.LogError("ARCharacterTracker not found in the scene!");
            }
        }
        else
        {
            Debug.LogError("VR Character Prefab Name is missing!");
        }
    }
}
