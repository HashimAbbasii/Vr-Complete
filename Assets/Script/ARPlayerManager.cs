using UnityEngine;
using Photon.Pun;

public class ARPlayerManager : MonoBehaviourPunCallbacks
{
    public string vrCharacterPrefabName = "Y Bot"; // Must match the VR prefab name
    public ARCharacterTracker arTracker; // Assign in Inspector

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("VR client - no need to spawn VR character in AR");
            return;
        }

        GameObject vrCharacter = PhotonNetwork.Instantiate(
            "Y Bot",
            Vector3.zero,
            Quaternion.identity
        );
        Debug.Log($"AR Spawned VR Character. ViewID: {vrCharacter.GetComponent<PhotonView>().ViewID}");
    }
}