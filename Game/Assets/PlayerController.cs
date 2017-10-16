using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject cursorTarget;
    public float speed = 10.0F;
    public float zoomSpeed = 10.0F;
    public float zoomSmooth = 10.0F;

    public float dragSpeed = 2;
    //private Vector3 dragOrigin;

    public float rotateSensitivity;


    public float maxZoom;
    public float minZoom;

    private float zoom;
    private bool isDragging;

    private Vector3 onClickPoint;

    public LayerMask dragMask;
    RaycastHit hit;
    Camera cam;
    public MoveState moveState;

    bool rotating = false;

    private Vector3 mouseDownPoint;

    private readonly Vector2 tiltBounds = new Vector2(30, 90); 

    public enum MoveState
    {
        ClickDrag = 0,
        ScreenEdgePan = 1,
        Pan,
        Tilt,
        Rotate
    };

    void Start()
    {
        cam = Camera.main;
        zoom = cam.orthographicSize;
    }

    void Update()
    {
        rotating = Input.GetMouseButton(1);
        /*
        switch (moveState)
        {
            case MoveState.ClickDrag:
                Debug.Log("Blah");
                break;
            case MoveState.ScreenEdgePan:
                Debug.Log("Blee");
                break;
            case MoveState.Pan:
                Debug.Log("Bloo");
                break;
            case MoveState.Tilt:
                Debug.Log("Blit");
                break;
            case MoveState.Rotate:
                Debug.Log("Bliss");
                break;
            default:
                Debug.Log("Blam");
                break;
        }
        */
        float x = Input.GetAxis("Vertical");
        float y = Input.GetAxis("Horizontal");
        x *= Time.deltaTime;
        y *= Time.deltaTime;
        Vector3 shiftMove = new Vector3(y, 0, x);

        zoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, zoom, Time.deltaTime * zoomSpeed);

        if (Input.GetMouseButtonDown(1))
            mouseDownPoint = Input.mousePosition;

        if (rotating)
        {
            var mouseHorzDelta = mouseDownPoint.x - Input.mousePosition.x;
            var mouseVertDelta = mouseDownPoint.y - Input.mousePosition.y;
            transform.RotateAround(cursorTarget.transform.position, Vector3.up, -mouseHorzDelta * rotateSensitivity);
            transform.RotateAround(cursorTarget.transform.position, transform.right, mouseVertDelta * rotateSensitivity);
            mouseDownPoint = Input.mousePosition;
        }
        
        if(!rotating)
        {
            transform.Translate(shiftMove * speed, Space.Self);
        }
    }

    void LateUpdate()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 1000) && !rotating)
        {
            cursorTarget.transform.position = hit.point;
        }
    }
}
