using UnityEngine;

public class VRMovement : MonoBehaviour
{
    public float moveSpeed = 3f;

    void Update()
    {
        // Simulate VR movement with keyboard (WASD)
        float x = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float z = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        transform.Translate(x, 0, z);
    }
}