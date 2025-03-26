using UnityEngine;
using Photon.Pun;

public class ARNetworkCharacter : MonoBehaviourPun, IPunObservable
{
    [Tooltip("Smoothing speed for position/rotation updates")]
    public float smoothSpeed = 10f; // Increased for better responsiveness

    private Vector3 networkPosition;
    private Quaternion networkRotation;

    void Update()
    {
        // Debug ownership issues
        if (photonView.IsMine)
        {
            Debug.LogError($"FATAL: AR client incorrectly owns VR character! ViewID: {photonView.ViewID}");
            return;
        }

        // Smoothly interpolate to received network values
        transform.position = Vector3.Lerp(transform.position, networkPosition, Time.deltaTime * smoothSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, networkRotation, Time.deltaTime * smoothSpeed);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // VR player writes data
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            Debug.Log($"VR→AR Sync | Pos: {transform.position}");
        }
        else
        {
            // AR player reads data
            networkPosition = (Vector3)stream.ReceiveNext();
            networkRotation = (Quaternion)stream.ReceiveNext();
            Debug.Log($"AR←VR Update | Pos: {networkPosition}");
        }
    }
}