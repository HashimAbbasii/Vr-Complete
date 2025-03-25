using UnityEngine;
using Photon.Pun;

public class VRNetworkCharacter : MonoBehaviourPun, IPunObservable
{
    [Header("Settings")]
    public float smoothSpeed = 5f;

    void Update()
    {
        if (!photonView.IsMine) return;

        // Example movement for testing (Replace with actual VR tracking)
        float moveX = Input.GetAxis("Horizontal") * Time.deltaTime * 3f;
        float moveZ = Input.GetAxis("Vertical") * Time.deltaTime * 3f;
        transform.position += new Vector3(moveX, 0, moveZ);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting && photonView.IsMine) // VR Player sending data
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            Debug.Log("VR Sent Data: " + transform.position);
        }
        else // AR Player receiving data
        {
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
            Debug.Log("AR Received Data: " + transform.position);
        }
    }
}
