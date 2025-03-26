using UnityEngine;
using Photon.Pun;

public class VRNetworkCharacter : MonoBehaviourPunCallbacks, IPunObservable
{
    private Vector3 networkPosition;
    private Quaternion networkRotation;
    public float smoothingSpeed = 10f;

    void Update()
    {
        // Smooth interpolation for non-local players
        if (!photonView.IsMine)
        {
            transform.position = Vector3.Lerp(transform.position, networkPosition, Time.deltaTime * smoothingSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, networkRotation, Time.deltaTime * smoothingSpeed);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Send position and rotation
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            // Receive position and rotation
            networkPosition = (Vector3)stream.ReceiveNext();
            networkRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}