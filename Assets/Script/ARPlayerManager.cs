using UnityEngine;
using Photon.Pun;

public class ARPlayerManager : MonoBehaviourPunCallbacks
{
    public string vrCharacterPrefabName = "Y Bot"; // Ensure this matches the exact prefab name

    public override void OnJoinedRoom()
    {
        Debug.Log("AR Player Joined Room!");

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Master Client (VR) detected, AR will not instantiate.");
            return;
        }

        if (!string.IsNullOrEmpty(vrCharacterPrefabName))
        {
            GameObject vrCharacter = PhotonNetwork.Instantiate(vrCharacterPrefabName, Vector3.zero, Quaternion.identity);
            Debug.Log("Spawned VR Character in AR Scene: " + vrCharacter.name);
        }
        else
        {
            Debug.LogError("VR Character Prefab Name is missing!");
        }
    }
}
