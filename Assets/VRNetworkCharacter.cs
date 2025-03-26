using UnityEngine;
using Photon.Pun;

public class VRNetworkCharacter : MonoBehaviourPun, IPunObservable
{
    private Vector3 lastSentPosition;
    private float lastSendTime;
    public float sendInterval = 0.1f; // How often to sync (seconds)

    void Start()
    {
        // Ownership validation
        if (!photonView.IsMine)
        {
            if (PhotonNetwork.IsConnected)
                PhotonNetwork.Destroy(gameObject);
            return;
        }
        Debug.Log($"VR Character - Correct Owner | ViewID: {photonView.ViewID}");
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        // Debug ownership
        Debug.Log($"VR owns: {gameObject.name} | ViewID: {photonView.ViewID}");

        // Your VR movement input handling here
        // float moveX = Input.GetAxis("Horizontal") * Time.deltaTime * 3f;
        // float moveZ = Input.GetAxis("Vertical") * Time.deltaTime * 3f;
        // transform.position += new Vector3(moveX, 0, moveZ);

        // Manual sync with rate limiting
        if (Time.time - lastSendTime > sendInterval ||
            Vector3.Distance(transform.position, lastSentPosition) > 0.1f)
        {
            photonView.RPC("ForceSync", RpcTarget.Others, transform.position, transform.rotation);
            lastSentPosition = transform.position;
            lastSendTime = Time.time;
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
    void ForceSync(Vector3 pos, Quaternion rot)
    {
        // Only non-owners should execute this
        if (!photonView.IsMine)
        {
            transform.position = pos;
            transform.rotation = rot;
        }
    }
}