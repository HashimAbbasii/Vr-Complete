using UnityEngine;
using Photon.Pun;

public class ARCharacterTracker : MonoBehaviourPun, IPunObservable
{
    public Transform vrTarget; // ✅ Make sure this is PUBLIC

    public float smoothSpeed = 5f;
    private Vector3 networkPosition;
    private Quaternion networkRotation;

    void Update()
    {
        if (!photonView.IsMine) // AR Player receives VR player updates
        {
            if (vrTarget != null)
            {
                transform.position = Vector3.Lerp(transform.position, networkPosition, Time.deltaTime * smoothSpeed);
                transform.rotation = Quaternion.Slerp(transform.rotation, networkRotation, Time.deltaTime * smoothSpeed);
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) // VR sends position updates
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else // AR receives position updates
        {
            networkPosition = (Vector3)stream.ReceiveNext();
            networkRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
