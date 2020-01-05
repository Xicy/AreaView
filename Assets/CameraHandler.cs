using System;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    private const float RotateSpeed = 200f;
    private const float ZoomSpeedTouch = 0.1f;
    private const float ZoomSpeedMouse = 10f;

    private static readonly float[] BoundsY = new[] { -90f, 90f };
    private static readonly float[] ZoomBounds = new[] { 10f, 85f };

    private Camera _cam;

    private Vector3 _lastPanPosition;
    private Vector3 _lastRotation;
    private int _panFingerId; // Touch mode only
    private bool _wasZoomingLastFrame; // Touch mode only
    private Vector2[] _lastZoomPositions; // Touch mode only

    void Awake()
    {
        _cam = GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.touchSupported && Application.platform != RuntimePlatform.WebGLPlayer)
        {
            HandleTouch();
        }
        else
        {
            HandleMouse();
        }
    }

    void HandleTouch()
    {
        switch (Input.touchCount)
        {

            case 1: // Panning
                _wasZoomingLastFrame = false;

                // If the touch began, capture its position and its finger ID.
                // Otherwise, if the finger ID of the touch doesn't match, skip it.
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    _lastRotation = transform.eulerAngles;
                    _lastPanPosition = touch.position;
                    _panFingerId = touch.fingerId;
                }
                else if (touch.fingerId == _panFingerId && touch.phase == TouchPhase.Moved)
                {
                    RotateCamera(touch.position);
                }
                break;

            case 2: // Zooming
                Vector2[] newPositions = new Vector2[] { Input.GetTouch(0).position, Input.GetTouch(1).position };
                if (!_wasZoomingLastFrame)
                {
                    _lastZoomPositions = newPositions;
                    _wasZoomingLastFrame = true;
                }
                else
                {
                    // Zoom based on the distance between the new positions compared to the 
                    // distance between the previous positions.
                    float newDistance = Vector2.Distance(newPositions[0], newPositions[1]);
                    float oldDistance = Vector2.Distance(_lastZoomPositions[0], _lastZoomPositions[1]);
                    float offset = newDistance - oldDistance;

                    ZoomCamera(offset, ZoomSpeedTouch);

                    _lastZoomPositions = newPositions;
                }
                break;

            default:
                _wasZoomingLastFrame = false;
                break;
        }
    }

    void HandleMouse()
    {
        // On mouse down, capture it's position.
        // Otherwise, if the mouse is still down, pan the camera.
        if (Input.GetMouseButtonDown(0))
        {
            _lastRotation = transform.eulerAngles;
            _lastPanPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            RotateCamera(Input.mousePosition);
        }

        // Check for scrolling to zoom the camera
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        ZoomCamera(scroll, ZoomSpeedMouse);
    }

    void RotateCamera(Vector3 newPanPosition)
    {
        var offset = _cam.ScreenToViewportPoint(_lastPanPosition - newPanPosition) * RotateSpeed;

        var angle = _lastRotation.x;
        if (angle > 180)
            angle -= 360;

        offset.y = Mathf.Clamp(angle - offset.y, BoundsY[0], BoundsY[1]);
        transform.rotation = Quaternion.Euler( offset.y, offset.x + _lastRotation.y, 0);
    }

    void ZoomCamera(float offset, float speed)
    {
        if (Math.Abs(offset) < 0.01)
            return;

        _cam.fieldOfView = Mathf.Clamp(_cam.fieldOfView - (offset * speed), ZoomBounds[0], ZoomBounds[1]);
    }

}