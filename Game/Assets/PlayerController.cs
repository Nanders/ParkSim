using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject onClickObject;
    public GameObject boundingSphere;
    private SphereCollider boundingCollider;

    public LayerMask boundingMask;

    [Range(1, 20)]
    public float speed = 10.0F;
    public float zoomSpeed = 10.0F;
    public float zoomSmooth = 10.0F;
    public bool freeLook;

    public float boundary;

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

    private bool rotating = false;

    private Vector3 mouseDownPoint;
    private Vector3 rotateCenter;

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
        Cursor.lockState = CursorLockMode.Confined;
        cam = Camera.main;
        zoom = cam.orthographicSize;
        boundingCollider = boundingSphere.GetComponent<SphereCollider>();
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
        float y = Input.GetAxis("Vertical");
        float x = Input.GetAxis("Horizontal");
        y *= Time.deltaTime;
        x *= Time.deltaTime;
        

        zoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;

        zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoom, Time.deltaTime * zoomSmooth);
        boundingCollider.radius = cam.orthographicSize * 2f;

        if (Input.GetMouseButtonDown(1))
            mouseDownPoint = Input.mousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            var action = new CreateAction(onClickObject, rotateCenter, Quaternion.identity);
            action.Do();
            ActionManager.obj.PushAction(action);          
        }

        if (rotating)
        {
            var mouseHorzDelta = mouseDownPoint.x - Input.mousePosition.x;
            var mouseVertDelta = mouseDownPoint.y - Input.mousePosition.y;
            var target = freeLook ? this.transform.position : rotateCenter;

            var mouseHorzScaled = mouseHorzDelta * rotateSensitivity;
            var mouseVertScaled = mouseVertDelta * rotateSensitivity;

            //Y axis rotation
            transform.RotateAround(target, Vector3.up, -mouseHorzScaled);

            //Clamp X rotation
            if (transform.rotation.eulerAngles.x + mouseVertScaled > minPitch && 
                transform.rotation.eulerAngles.x + mouseVertScaled < maxPitch)
            {
                transform.RotateAround(target, transform.right, mouseVertScaled);
            }   

            mouseDownPoint = Input.mousePosition;
        }
        
        if(!rotating)
        {
            var edgePan = EdgePan(x, y);
            y = edgePan.y;
            x = edgePan.x;

            Vector3 shiftMove = new Vector3(x, 0, y);
            transform.Translate(Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.up) * (shiftMove * speed), Space.World);
        }
    }

    void LateUpdate()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 1000, interactMask) && !rotating)
        {
            rotateCenter = hit.point;         
        }

        if (Physics.Raycast(cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)), out hit, 1000, interactMask))
        {
            boundingSphere.transform.position = hit.point;
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

    Vector2 EdgePan(float x, float y)
    {
        float xDelta = 0;
        float yDelta = 0;

        if (Input.mousePosition.x > Screen.width - boundary)
        {
            xDelta = Mathf.Abs(Input.mousePosition.x - (Screen.width - boundary));
            xDelta = xDelta.Remap(0, boundary, 0, 1);
            xDelta = Mathf.Clamp(xDelta, 0, 1); //this can happen in unity when the mouse is not bounded to screen

            x += xDelta * Time.deltaTime;
        }

        if (Input.mousePosition.x < boundary)
        {
            xDelta = Mathf.Abs(Input.mousePosition.x - boundary);
            xDelta = xDelta.Remap(0, boundary, 0, 1);
            xDelta = Mathf.Clamp(xDelta, 0, 1); //this can happen in unity when the mouse is not bounded to screen
            x -= xDelta * Time.deltaTime;
        }

        if (Input.mousePosition.y > Screen.height - boundary)
        {
            yDelta = Mathf.Abs(Input.mousePosition.y - (Screen.height - boundary));
            yDelta = yDelta.Remap(0, boundary, 0, 1);
            yDelta = Mathf.Clamp(yDelta, 0, 1); //this can happen in unity when the mouse is not bounded to screen

            y += yDelta * Time.deltaTime;
        }

        if (Input.mousePosition.y < boundary)
        {
            yDelta = Mathf.Abs(Input.mousePosition.y - boundary);
            yDelta = yDelta.Remap(0, boundary, 0, 1);
            yDelta = Mathf.Clamp(yDelta, 0, 1); //this can happen in unity when the mouse is not bounded to screen
            y -= yDelta * Time.deltaTime;
        }
        
        return new Vector2(x, y);
    }
}

//TODO: Move to new place
public static class ExtensionMethods
{
    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
