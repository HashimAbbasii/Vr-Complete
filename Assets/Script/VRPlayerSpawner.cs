using UnityEngine;
using Photon.Pun;

public class VRPlayerSpawner : MonoBehaviourPunCallbacks
{
    void Start()
    {
        if (PhotonNetwork.IsMasterClient) // Only the VR player spawns this
        {
            GameObject vrPlayer = PhotonNetwork.Instantiate(
                "Y Bot",
                transform.position, // Spawns at this object's position
                Quaternion.identity
            );
            Debug.Log($"VR Spawned. ViewID: {vrPlayer.GetComponent<PhotonView>().ViewID}");
        }
    }
}