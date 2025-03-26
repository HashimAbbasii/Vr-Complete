using UnityEngine;
using Photon.Pun;

public class ARPlayerManager : MonoBehaviourPunCallbacks
{
    public ARCharacterTracker arTracker; // Assign in Inspector
    public string vrCharacterTag = "VRCharacter"; // Tag for VR character

    public override void OnJoinedRoom()
    {
        // Ensure this only runs on the AR client
        if (!PhotonNetwork.IsMasterClient)
        {
            // Find the VR character in the scene
            GameObject vrCharacter = GameObject.FindGameObjectWithTag(vrCharacterTag);

            if (vrCharacter != null)
            {
                // Set the tracker to follow the VR character
                if (arTracker != null)
                {
                    arTracker.SetVRCharacterTransform(vrCharacter.transform);
                    Debug.Log("AR Tracker initialized to follow VR character");
                }
            }
            else
            {
                Debug.LogWarning("No VR character found with the specified tag!");
            }
        }
    }
}