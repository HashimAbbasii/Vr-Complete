using UnityEngine;
using Photon.Pun;

public class ARCharacterTracker : MonoBehaviourPun, IPunObservable
{
    public Transform vrTarget; // Assign this in ARPlayerManager
    public float smoothSpeed = 10f; // Increased for better responsiveness

    private Vector3 networkPosition;
    private Quaternion networkRotation;
    private void Start()
    {
        if (vrTarget != null)
        {
            PhotonView vrPhotonView = vrTarget.GetComponent<PhotonView>();
            Debug.Log($"AR Tracking VR Character ViewID: {vrPhotonView?.ViewID}");
        }
    }

    void Update()
    {
        if (!photonView.IsMine && vrTarget != null)
        {
            transform.position = Vector3.Lerp(transform.position, networkPosition, Time.deltaTime * smoothSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, networkRotation, Time.deltaTime * smoothSpeed);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting && photonView.IsMine && vrTarget != null)
        {
            stream.SendNext(vrTarget.position);
            stream.SendNext(vrTarget.rotation);
        }
        else
        {
            networkPosition = (Vector3)stream.ReceiveNext();
            networkRotation = (Quaternion)stream.ReceiveNext();
            Debug.Log("AR received position: " + networkPosition);
        }
    }
}