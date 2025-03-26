using UnityEngine;
using Photon.Pun;

public class ARCharacterTracker : MonoBehaviourPunCallbacks
{
    public Transform vrCharacterTransform; // Reference to the VR character's transform
    public float smoothSpeed = 10f;

    void Update()
    {
        // If VR character exists and is not local to this client
        if (vrCharacterTransform != null)
        {
            // Smoothly follow the VR character's position and rotation
            transform.position = Vector3.Lerp(transform.position, vrCharacterTransform.position, Time.deltaTime * smoothSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, vrCharacterTransform.rotation, Time.deltaTime * smoothSpeed);
        }
    }

    // Method to set the VR character reference
    public void SetVRCharacterTransform(Transform vrTransform)
    {
        vrCharacterTransform = vrTransform;
        Debug.Log("AR Tracker now following VR character");
    }
}