using UnityEngine;
using Photon.Pun;

public class ARCharacterTracker : MonoBehaviourPun, IPunObservable
{
    [Header("Settings")]
    [Tooltip("The VR character's transform to follow")]
    public Transform vrTarget; // Assigned by ARPlayerManager

    [Tooltip("Smoothing speed for movement")]
    [Range(5f, 30f)] public float smoothSpeed = 15f;

    // Networked values
    private Vector3 networkPosition;
    private Quaternion networkRotation;
    private float lastNetworkTime;

    void Update()
    {
        // Only interpolate if we're not the owner (AR client)
        if (!photonView.IsMine && vrTarget != null)
        {
            float timeSinceUpdate = Time.time - lastNetworkTime;
            float lerpFactor = Mathf.Clamp01(timeSinceUpdate * smoothSpeed);

            transform.position = Vector3.Lerp(transform.position, networkPosition, lerpFactor);
            transform.rotation = Quaternion.Slerp(transform.rotation, networkRotation, lerpFactor);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // VR player writes position data
            if (vrTarget != null)
            {
                stream.SendNext(vrTarget.position);
                stream.SendNext(vrTarget.rotation);
                Debug.Log($"VR→AR Sync | Pos: {vrTarget.position}");
            }
        }
        else
        {
            // AR player reads position data
            networkPosition = (Vector3)stream.ReceiveNext();
            networkRotation = (Quaternion)stream.ReceiveNext();
            lastNetworkTime = Time.time;

            Debug.Log($"AR←VR Update | Pos: {networkPosition}");
        }
    }

    void OnValidate()
    {
        // Auto-find tracker if not assigned
        if (vrTarget == null)
            vrTarget = transform;
    }
}