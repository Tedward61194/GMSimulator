using UnityEngine;
using Mirror;

public class GMCameraController : MonoBehaviour {
    public Transform cameraTransform;

    [SerializeField] float movementSpeed;
    [SerializeField] float movementTime;
    [SerializeField] float rotationAmount;
    [SerializeField] Vector3 zoomAmount;

    [SerializeField] float snapDistance;

    Vector3 newPosition;
    Quaternion newRotation;
    Vector3 newZoom;

    Vector3 dragStartPosition;
    Vector3 dragCurrentPosition;
    Vector3 rotateStartPosition;
    Vector3 rotateCurrentPosition;

    void Start() {
        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;

        if (GetComponentInParent<NetworkIdentity>().isLocalPlayer) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        } else {
            // Only allow client camera to be active
            cameraTransform.GetComponent<Camera>().enabled = false;
            cameraTransform.GetComponent<AudioListener>().enabled = false;
        }
    }

    void Update() {
        if (GetComponentInParent<NetworkIdentity>().isLocalPlayer) {
            HandleMovementInput();
            HandleMouseInput();
        }
    }

    void HandleMouseInput() {
        //Plane plane = new Plane(Vector3.up, Vector3.zero);
        //Ray ray = GameObject.FindGameObjectWithTag("GMCamera").GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        //float entry;

        // Mouse zoom
        if (Input.mouseScrollDelta.y != 0) {
            newZoom += Input.mouseScrollDelta.y * zoomAmount;
        }

        // Drag scroll
        // Initial press
        if (Input.GetMouseButtonDown(2)) {

            //if (plane.Raycast(ray, out entry)) {
            dragStartPosition = GetMousePos();
            //}
        }
        // Still holding button down
        if (Input.GetMouseButton(2)) {
            //if (plane.Raycast(ray, out entry)) {
            dragCurrentPosition = GetMousePos();//ray.GetPoint(entry);
            newPosition = transform.position + dragStartPosition - dragCurrentPosition;
            //}
        }

        // Drag rotate
        if (Input.GetMouseButtonDown(1)) {
            rotateStartPosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(1)) {
            rotateCurrentPosition = Input.mousePosition;
            Vector3 difference = rotateStartPosition - rotateCurrentPosition;
            rotateStartPosition = rotateCurrentPosition;
            newRotation *= Quaternion.Euler(Vector3.up * (-difference.x * rotationAmount));
        }

        // Build Wall
        // Will likely move this to another script
        

    }

    void HandleMovementInput() {
        // Pan
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            newPosition += (transform.forward * movementSpeed);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            newPosition += (transform.forward * -movementSpeed);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            newPosition += (transform.right * movementSpeed);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            newPosition += (transform.right * -movementSpeed);
        }

        // Rotate
        if (Input.GetKey(KeyCode.Q)) {
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        }
        if (Input.GetKey(KeyCode.E)) {
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
        }

        // Zoom
        if (Input.GetKey(KeyCode.R)) {
            newZoom += zoomAmount;
        }
        if (Input.GetKey(KeyCode.F)) {
            newZoom -= zoomAmount;
        }

        //Apply new transform
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);
    }

    public Vector3 GetMousePos() {
        Ray ray = GameObject.FindGameObjectWithTag("GMCamera").GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) {
            return hit.point;
        }
        return Vector3.zero;
    }

    public Vector3 GetMousePosIgnoreLayer(LayerMask layerToIgnore) {
        Ray ray = GameObject.FindGameObjectWithTag("GMCamera").GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~layerToIgnore)) {
            return hit.point;
        }
        return Vector3.zero;
    }

    public GameObject GetObjectAtMousePosition(LayerMask layer) {
        Ray ray = GameObject.FindGameObjectWithTag("GMCamera").GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer)) {
            return hit.transform.root.gameObject;
        }
        return null;
    }

    public Vector3 SnapPosition(Vector3 origional) {
        Vector3 snappedPos;
        snappedPos.x = Mathf.Floor(origional.x + snapDistance);
        snappedPos.y = Mathf.Floor(origional.y + snapDistance);
        snappedPos.z = Mathf.Floor(origional.z + snapDistance);
        return snappedPos;
    }
}
