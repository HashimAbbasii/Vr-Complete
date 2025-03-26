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
        if (!photonView.IsMine) return;

        // Debug current position and ownership status
        Debug.Log($"VR Position: {transform.position} | IsMine: {photonView.IsMine}");

        // Your VR movement logic here (replace with actual VR input)
        float moveX = Input.GetAxis("Horizontal") * Time.deltaTime * 3f;
        float moveZ = Input.GetAxis("Vertical") * Time.deltaTime * 3f;
        transform.position += new Vector3(moveX, 0, moveZ);
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
}