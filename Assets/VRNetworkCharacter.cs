using UnityEngine;
using Photon.Pun;

public class VRNetworkCharacter : MonoBehaviourPun, IPunObservable
{
    [Header("Settings")]
    public float smoothSpeed = 5f;

    private Vector3 networkPosition;
    private Quaternion networkRotation;

    void Update()
    {
        if (photonView.IsMine)
        {
            float moveX = Input.GetAxis("Horizontal") * Time.deltaTime * 3f;
            float moveZ = Input.GetAxis("Vertical") * Time.deltaTime * 3f;
            transform.position += new Vector3(moveX, 0, moveZ);
        }
        else // AR Client receives movement updates
        {
            transform.position = Vector3.Lerp(transform.position, networkPosition, Time.deltaTime * smoothSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, networkRotation, Time.deltaTime * smoothSpeed);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting && photonView.IsMine)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            Debug.Log("VR Sent Data: " + transform.position);
        }
        else
        {
            networkPosition = (Vector3)stream.ReceiveNext();
            networkRotation = (Quaternion)stream.ReceiveNext();
            Debug.Log("AR Received Data: " + networkPosition);
        }
    }
}
