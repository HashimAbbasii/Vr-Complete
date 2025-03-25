using UnityEngine;
using Photon.Pun;

public class ARNetworkCharacter : MonoBehaviourPun, IPunObservable
{
    [Header("Settings")]
    public float smoothSpeed = 5f;

    [Header("Debug")]
    [SerializeField] private Vector3 networkPosition;
    [SerializeField] private Quaternion networkRotation;
    [SerializeField] private bool isReceivingData;

    void Update()
    {
        if (!photonView.IsMine)
        {
            if (isReceivingData) // Only update if we got network data
            {
                transform.position = Vector3.Lerp(transform.position, networkPosition, Time.deltaTime * smoothSpeed);
                transform.rotation = Quaternion.Slerp(transform.rotation, networkRotation, Time.deltaTime * smoothSpeed);
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting && photonView.IsMine) // VR Player sending
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            Debug.Log("VR: Sending position");
        }
        else // AR Player receiving
        {
            networkPosition = (Vector3)stream.ReceiveNext();
            networkRotation = (Quaternion)stream.ReceiveNext();
            isReceivingData = true;
            Debug.Log($"AR: Received pos: {networkPosition}");
        }
    }
}