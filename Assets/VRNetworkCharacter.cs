using UnityEngine;
using Photon.Pun;

public class VRNetworkCharacter : MonoBehaviourPun, IPunObservable
{
    void Update()
    {
        if (!photonView.IsMine) return;

        // Example VR movement (Replace with actual VR tracking)
        float moveX = Input.GetAxis("Horizontal") * Time.deltaTime * 3f;
        float moveZ = Input.GetAxis("Vertical") * Time.deltaTime * 3f;
        transform.position += new Vector3(moveX, 0, moveZ);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) // VR Player sending data
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
    }
}
