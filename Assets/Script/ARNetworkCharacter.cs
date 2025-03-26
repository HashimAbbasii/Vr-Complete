using UnityEngine;
using Photon.Pun;

public class ARNetworkCharacter : MonoBehaviourPun, IPunObservable
{
    public float smoothSpeed = 5f;

    private Vector3 networkPosition;
    private Quaternion networkRotation;

    void Update()
    {
        if (!photonView.IsMine) // AR Player receiving movement
        {
            transform.position = Vector3.Lerp(transform.position, networkPosition, Time.deltaTime * smoothSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, networkRotation, Time.deltaTime * smoothSpeed);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting && photonView.IsMine) // VR Player sending data
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            Debug.Log("VR: Sending position");
        }
        else // AR Player receiving data
        {
            networkPosition = (Vector3)stream.ReceiveNext();
            networkRotation = (Quaternion)stream.ReceiveNext();
            Debug.Log($"AR: Received pos: {networkPosition}");
        }
    }
}
