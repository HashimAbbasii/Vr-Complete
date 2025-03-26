using UnityEngine;
using Photon.Pun;

public class ARPlayerManager : MonoBehaviourPunCallbacks
{
    public string vrCharacterPrefabName = "Y Bot";

    public override void OnJoinedRoom()
    {
        // Only non-master (AR) client spawns VR character
        if (!PhotonNetwork.IsMasterClient)
        {
            GameObject vrCharacter = PhotonNetwork.Instantiate(
                vrCharacterPrefabName,
                Vector3.zero,
                Quaternion.identity
            );
            Debug.Log($"AR Spawned VR Character. ViewID: {vrCharacter.GetComponent<PhotonView>().ViewID}");
        }
    }
}