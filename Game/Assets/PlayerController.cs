using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject focus;
    public GameObject onClickObject;
    public GameObject boundingSphere;
    private SphereCollider boundingCollider;

    public LayerMask boundingMask;
    public LayerMask snappingMask;

    [Range(1, 20)]
    public float speed = 10.0F;
    public float zoomSpeed = 10.0F;
    public float zoomSmooth = 10.0F;
    public bool freeLook;

    public float boundary;

    public float rotateSensitivity;

    public float maxZoom = 100f;
    public float minZoom = 0f;

    public float maxAngle = 90f;
    public float minAngle = 30f;

    private float zoom;

    public LayerMask terrainDragMask;
    RaycastHit hit;
    Camera cam;


    private bool rotating = false;

    private Vector3 lastMousePoint;
    private Vector3 terrainHitPoint;

    Ray mousePositionRay;
    Ray screenCenterRay;

    //TODO: move interact states to this
    public State state;
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
        Cursor.lockState = CursorLockMode.Confined; //Note, this doesnt work in editor
        cam = Camera.main;
        zoom = cam.orthographicSize;
        boundingCollider = boundingSphere.GetComponent<SphereCollider>();
    }

    void Update()
    {
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
        
        //TODO: unfuck zoom code
        //TODO: figure out ortho or use perspective
        zoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;

        zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoom, Time.deltaTime * zoomSmooth);
        boundingCollider.radius = cam.orthographicSize;

        if (Input.GetMouseButtonDown(1))
            lastMousePoint = Input.mousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            focus = new CreateAction(onClickObject, terrainHitPoint, Quaternion.identity).createdGo;        
        }

        if (Input.GetMouseButton(0))
        {
            RaycastHit hitAgain;
            if (Physics.Raycast(mousePositionRay, out hitAgain, 1000f, snappingMask))
            {
                if (hitAgain.collider.gameObject.transform.root.gameObject != focus)
                    focus.transform.position = hitAgain.collider.gameObject.transform.position;
                else
                    focus.transform.position = terrainHitPoint;
            }
            else
                focus.transform.position = terrainHitPoint;
        }

        //pan around
        if (Input.GetMouseButton(1))
        {
            
            var mouseHorzDelta = lastMousePoint.x - Input.mousePosition.x;
            var mouseVertDelta = lastMousePoint.y - Input.mousePosition.y;
            var target = freeLook ? this.transform.position : terrainHitPoint; //rotate around the mouseclick or our origin

            var mouseHorzScaled = mouseHorzDelta * rotateSensitivity;
            var mouseVertScaled = mouseVertDelta * rotateSensitivity;

            //Y axis rotation
            transform.RotateAround(target, Vector3.up, -mouseHorzScaled);

            //Clamp X rotation
            if (transform.rotation.eulerAngles.x + mouseVertScaled > minAngle && 
                transform.rotation.eulerAngles.x + mouseVertScaled < maxAngle)
            {
                transform.RotateAround(target, transform.right, mouseVertScaled);
            }   

            //update this continuously
            lastMousePoint = Input.mousePosition;
        }
        else
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
        mousePositionRay = cam.ScreenPointToRay(Input.mousePosition);
        screenCenterRay = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(mousePositionRay, out hit, 1000, terrainDragMask) && !rotating)
        {
            terrainHitPoint = hit.point;         
        }

        if (Physics.Raycast(screenCenterRay, out hit, 1000, terrainDragMask))
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
