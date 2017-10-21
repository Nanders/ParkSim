using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject onClickObject;
    public GameObject cursorTarget;
    public LayerMask boundingMask;
    public float speed = 10.0F;
    public float zoomSpeed = 10.0F;
    public float zoomSmooth = 10.0F;
    public bool freeLook;

    public float rotateSensitivity;


    public float maxZoom = 100f;
    public float minZoom = 0f;

    public float maxPitch = 90f;
    public float minPitch = 30f;

    private float zoom;

    public LayerMask interactMask;
    RaycastHit hit;
    Camera cam;
    public State state;

    bool rotating = false;

    private Vector3 mouseDownPoint;

    public enum State
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
        switch (state)
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

        zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoom, Time.deltaTime * zoomSmooth);

        if (Input.GetMouseButtonDown(1))
            mouseDownPoint = Input.mousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            var action = new CreateAction(onClickObject, cursorTarget.transform.position, Quaternion.identity);
            action.Do();
            ActionManager.obj.PushAction(action);          
        }

        if (rotating)
        {
            var mouseHorzDelta = mouseDownPoint.x - Input.mousePosition.x;
            var mouseVertDelta = mouseDownPoint.y - Input.mousePosition.y;
            var target = freeLook ? this.transform.position : cursorTarget.transform.position;

            //Y axis rotation
            transform.RotateAround(target, Vector3.up, -mouseHorzDelta * rotateSensitivity);

            //Clamp X rotation
            if (transform.rotation.eulerAngles.x + mouseVertDelta * rotateSensitivity > minPitch && 
                transform.rotation.eulerAngles.x + mouseVertDelta * rotateSensitivity < maxPitch)
            {
                transform.RotateAround(target, transform.right, mouseVertDelta * rotateSensitivity);
            }   

            mouseDownPoint = Input.mousePosition;
        }
        
        if(!rotating)
        {
            transform.Translate(Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.up) * (shiftMove * speed), Space.World);
        }
    }

    void LateUpdate()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 1000, interactMask) && !rotating)
        {
            cursorTarget.transform.position = hit.point;
        }
        AdjustToBounds();
    }

    void AdjustToBounds()
    {
        Ray boundingRay = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(boundingRay, out hit, 1000, boundingMask))
        {
            transform.position = hit.point;
        }
    }
}
