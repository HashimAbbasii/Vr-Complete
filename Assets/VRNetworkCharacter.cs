using UnityEngine;
using Photon.Pun;

public class VRNetworkCharacter : MonoBehaviourPun, IPunObservable
{
    private Vector3 lastSentPosition;
    private void Start()
    {
        Debug.Log($"VR Character ViewID: {photonView.ViewID} | IsMine: {photonView.IsMine}");
    }

    void Update()
    {
        // Only execute for the local VR player
        if (photonView.IsMine)
        {
            // Send updates manually if automatic sync fails
            photonView.RPC(
                "SyncTransform",
                RpcTarget.Others,
                transform.position,
                transform.rotation
            );
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting && photonView.IsMine)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            Debug.Log($"VR → Sending Position: {transform.position}");
        }
    }
    [PunRPC]
    void SyncTransform(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
    }

    //void Update()
    //{
    //    if (photonView.IsMine)
    //    {
    //        // Send updates manually if automatic sync fails
    //        photonView.RPC(
    //            "SyncTransform",
    //            RpcTarget.Others,
    //            transform.position,
    //            transform.rotation
    //        );
    //    }
    //}
}