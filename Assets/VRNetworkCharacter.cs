using UnityEngine;
using Photon.Pun;

public class VRNetworkCharacter : MonoBehaviourPun, IPunObservable
{
    public float smoothSpeed = 5f;

    private Vector3 networkPosition;
    private Quaternion networkRotation;

    void Update()
    {
        if (photonView.IsMine)
        {
            // Example movement for testing (Replace with VR tracking data)
            float moveX = Input.GetAxis("Horizontal") * Time.deltaTime * 3f;
            float moveZ = Input.GetAxis("Vertical") * Time.deltaTime * 3f;
            transform.position += new Vector3(moveX, 0, moveZ);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting && photonView.IsMine) // VR Player sending data
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
    }
}
