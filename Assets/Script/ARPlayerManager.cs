using UnityEngine;
using Photon.Pun;

public class ARPlayerManager : MonoBehaviourPunCallbacks
{
    public string vrCharacterPrefabName = "Y Bot"; // Ensure this matches the exact name in Resources

    public override void OnJoinedRoom()
    {
        Debug.Log("AR Player Joined Room!");

        if (PhotonNetwork.IsMasterClient) return; // VR Player should be master client, AR should not instantiate itself

        if (!string.IsNullOrEmpty(vrCharacterPrefabName))
        {
            GameObject vrCharacter = PhotonNetwork.Instantiate("Y Bot", Vector3.zero, Quaternion.identity);
            Debug.Log("Spawned VR Character in AR Scene: " + vrCharacter.name);
        }
        else
        {
            Debug.LogError("VR Character Prefab Name is missing!");
        }
    }
}
