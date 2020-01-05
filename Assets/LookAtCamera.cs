using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public Camera camera;

	void Update()
    {
        transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);
    }
}
