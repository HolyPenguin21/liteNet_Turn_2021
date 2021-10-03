using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameUI_Camera : MonoBehaviour
{
    private float PanSpeed = 5f;
    private float ZoomSpeedTouch = 0.005f;
    private float ZoomSpeedMouse = 2.0f;

    public float[] BoundsX = new float[]{-4f, 10f};
    public float[] BoundsY = new float[]{-4f, 4f};
    public float[] ZoomBounds = new float[]{1.25f, 5f};

    private Camera cam;

    private Vector3 lastPanPosition;
    private int panFingerId; // Touch mode only

    private bool wasZoomingLastFrame; // Touch mode only
    private Vector2[] lastZoomPositions; // Touch mode only

    private SceneMain_UI sceneMain_UI;

    void Awake()
    {
        cam = Camera.main;

        sceneMain_UI = GameObject.Find("SceneMain").GetComponent<SceneMain_UI>();
    }

    void LateUpdate()
    {
        if (sceneMain_UI.mouseOverUI) return;

        if (GameData.inst.inputPc)
            HandleMouse();
        else
            HandleTouch();
    }
    
    void HandleTouch()
    {
        switch(Input.touchCount)
        {
            case 1: // Panning
                wasZoomingLastFrame = false;
                
                // If the touch began, capture its position and its finger ID.
                // Otherwise, if the finger ID of the touch doesn't match, skip it.
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    lastPanPosition = touch.position;
                    panFingerId = touch.fingerId;
                } 
                else if (touch.fingerId == panFingerId && touch.phase == TouchPhase.Moved)
                {
                    PanCamera(touch.position);
                }
            break;

            case 2: // Zooming
                Vector2[] newPositions = new Vector2[]{Input.GetTouch(0).position, Input.GetTouch(1).position};
                
                if (!wasZoomingLastFrame)
                {
                    lastZoomPositions = newPositions;
                    wasZoomingLastFrame = true;
                }
                else
                {
                    // Zoom based on the distance between the new positions compared to the 
                    // distance between the previous positions.
                    float newDistance = Vector2.Distance(newPositions[0], newPositions[1]);
                    float oldDistance = Vector2.Distance(lastZoomPositions[0], lastZoomPositions[1]);
                    float offset = newDistance - oldDistance;

                    ZoomCamera(offset, ZoomSpeedTouch);

                    lastZoomPositions = newPositions;
                }
            break;
                
            default: 
                wasZoomingLastFrame = false;
            break;
        }
    }
    
    void HandleMouse()
    {
        // On mouse down, capture it's position.
        // Otherwise, if the mouse is still down, pan the camera.
        if (GameData.inst.panCamera)
        {
            if (Input.GetMouseButtonDown(0))
            {
                lastPanPosition = Input.mousePosition;
            }
            else if (Input.GetMouseButton(0))
            {
                PanCamera(Input.mousePosition);
            }
        }

        // Check for scrolling to zoom the camera
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        ZoomCamera(scroll, ZoomSpeedMouse);
    }

    void PanCamera(Vector3 newPanPosition)
    {
        // Determine how much to move the camera
        Vector3 offset = cam.ScreenToViewportPoint(lastPanPosition - newPanPosition);
        Vector3 move = new Vector3(offset.x * PanSpeed, 0, offset.y * PanSpeed);
        
        // Perform the movement
        transform.Translate(move, Space.World);
        
        // Ensure the camera remains within bounds.
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(transform.position.x, BoundsX[0], BoundsX[1]);
        pos.y = Mathf.Clamp(transform.position.y, BoundsY[0], BoundsY[1]);
        transform.position = pos;

        // Cache the position
        lastPanPosition = newPanPosition;
    }   

    void ZoomCamera(float offset, float speed)
    {
        if (offset == 0) return;
        
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - (offset * speed), ZoomBounds[0], ZoomBounds[1]);
        //cam.fieldOfView = Mathf.Clamp(cam.fieldOfView - (offset * speed), ZoomBounds[0], ZoomBounds[1]);
    }
}
