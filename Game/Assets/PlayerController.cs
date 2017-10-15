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

    public float maxZoom;
    public float minZoom;

    private float zoom;
    private bool isDragging;

    private Vector3 onClickPoint;

    public LayerMask dragMask;
    RaycastHit hit;
    Camera cam;
    public MoveState moveState;

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

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 1000))
        {
            cursorTarget.transform.position = hit.point;
        }

        zoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoom, Time.deltaTime * zoomSpeed);
       
        transform.Translate(shiftMove, Space.World);
    }
}
