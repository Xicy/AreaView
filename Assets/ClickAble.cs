using UnityEngine;

public class ClickAble : MonoBehaviour
{
    public Camera cam;
    public int Scene;
    private Quaternion lastPos;

    public void OnMouseDown()
    {
        lastPos = cam.transform.rotation;
    }

    public void OnMouseUp()
    {
        if (lastPos == cam.transform.rotation)
        {
            RenderSettings.skybox.mainTexture = Resources.Load<Texture>("Views/"+ Scene);
        }
    }
}
