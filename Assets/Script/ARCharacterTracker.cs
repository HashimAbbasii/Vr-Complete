using UnityEngine;
using Photon.Pun;

public class ARCharacterTracker : MonoBehaviourPun, IPunObservable
{
    public Transform vrTarget; // Reference to the VR player's object in Photon
    public float smoothSpeed = 5f;

    private Vector3 networkPosition;
    private Quaternion networkRotation;

    void Update()
    {
        if (!photonView.IsMine) // AR Player receiving VR player updates
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
        if (stream.IsReading) // Receiving VR player's data
        {
            networkPosition = (Vector3)stream.ReceiveNext();
            networkRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
