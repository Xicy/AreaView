using UnityEngine;

public class ClickEngine : MonoBehaviour
{
    public Camera mainKamera;
    private RaycastHit raycastHit;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainKamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out raycastHit))
            {
                Debug.Log(raycastHit.collider.gameObject);
                ClickAble ca = raycastHit.collider.gameObject.GetComponent<ClickAble>();
                if (ca != null)
                {
                    Debug.Log("clicked");
                }
            }
        }

    }
}
