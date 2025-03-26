using UnityEngine;
using Photon.Pun;

public class ARCharacterTracker : MonoBehaviourPun, IPunObservable
{
    public Transform vrTarget; // VR Character to follow
    public float smoothSpeed = 5f;

    private Vector3 networkPosition;
    private Quaternion networkRotation;

    void Update()
    {
        if (!photonView.IsMine) // AR Player receiving VR data
        {
            if (vrTarget != null)
            {
                transform.position = Vector3.Lerp(transform.position, networkPosition, Time.deltaTime * smoothSpeed);
                transform.rotation = Quaternion.Slerp(transform.rotation, networkRotation, Time.deltaTime * smoothSpeed);
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) // VR Player sends position updates
        {
            stream.SendNext(vrTarget.position);  // ✅ Ensure correct data type
            stream.SendNext(vrTarget.rotation);
        }
        else // AR Player receives VR data
        {
            object posObj = stream.ReceiveNext();
            object rotObj = stream.ReceiveNext();

            // ✅ Type Checking Before Casting
            if (posObj is Vector3 && rotObj is Quaternion)
            {
                networkPosition = (Vector3)posObj;
                networkRotation = (Quaternion)rotObj;
            }
            else
            {
               // Debug.LogError("Received invalid data in OnPhotonSerializeView!");
            }
        }
    }
}
