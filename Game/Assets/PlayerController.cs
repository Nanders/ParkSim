using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float boostMultiplier;
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
        
        float speedMod = Input.GetKey(KeyCode.LeftShift) ? speed * boostMultiplier : speed;
        float x = Input.GetAxis("Vertical") * speedMod;
        float y = Input.GetAxis("Horizontal") * speedMod;
        x *= Time.deltaTime;
        y *= Time.deltaTime;
        Vector3 shiftMove = new Vector3(y, 0, x);
        */
        zoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoom, Time.deltaTime * zoomSpeed);

        if (Input.GetMouseButtonDown(0))
        {
            //dragOrigin = Input.mousePosition;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100000, dragMask))
            {
                onClickPoint = hit.point;
                Debug.Log(hit.collider.name);
            }
        }

        if (Input.GetMouseButton(0))
        {

        }
        //Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        //Vector3 move = new Vector3(pos.x * dragSpeed, 0, pos.y * dragSpeed);

       
        //transform.Translate(shiftMove, Space.World);
    }
}
 
public class CameraDrag : MonoBehaviour
{



    void Update()
    {

    }


}
