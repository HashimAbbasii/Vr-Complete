using UnityEngine;
using Photon.Pun;

public class ARPlayerManager : MonoBehaviourPunCallbacks
{
    [Tooltip("Must match the exact name of the VR prefab in Resources")]
    public string vrCharacterPrefabName = "Y Bot";

    [Tooltip("Drag the ARCharacterTracker component here")]
    public ARCharacterTracker arTracker;

    public override void OnJoinedRoom()
    {
        Debug.Log($"AR joined room. IsMaster: {PhotonNetwork.IsMasterClient}");

        // Master client (VR player) should never spawn the AR's VR character
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("VR client detected - skipping VR character spawn in AR");
            return;
        }

        // Only spawn if no existing networked VR character
        if (GameObject.FindGameObjectWithTag("VRPlayer") == null)
        {
            Debug.Log("AR is spawning VR character...");

            // Spawn and immediately disown
            GameObject vrCharacter = PhotonNetwork.Instantiate(
                vrCharacterPrefabName,
                Vector3.zero,
                Quaternion.identity
            );

            // Critical: Ensure AR doesn't control this object
            PhotonView vrView = vrCharacter.GetComponent<PhotonView>();
            vrView.TransferOwnership(PhotonNetwork.MasterClient);

            // Assign to tracker if available
            if (arTracker != null)
            {
                arTracker.vrTarget = vrCharacter.transform;
                Debug.Log($"AR assigned VR character (ViewID: {vrView.ViewID}) to tracker");
            }
            else
            {
                Debug.LogError("ARCharacterTracker reference not set!");
            }
        }
    }
}