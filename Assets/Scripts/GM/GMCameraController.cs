using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMCameraController : MonoBehaviour {
    public Transform cameraTransform;
    [SerializeField] int playerId;
    [SerializeField] float movementSpeed;
    [SerializeField] float movementTime;
    [SerializeField] float rotationAmount;
    [SerializeField] Vector3 zoomAmount;

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

        if (PlayerPrefs.GetInt("playerCamera") == playerId) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void Update() {
        if (PlayerPrefs.GetInt("playerCamera") == playerId) {
            HandleMovementInput();
            HandleMouseInput();
        }
    }

    void HandleMouseInput() {
        // Mouse zoom
        if (Input.mouseScrollDelta.y != 0) {
            newZoom += Input.mouseScrollDelta.y * zoomAmount;
        }

        // Drag scroll
        if (Input.GetMouseButtonDown(0)) {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = GameObject.FindGameObjectWithTag("GMCamera").GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

            float entry;
            if (plane.Raycast(ray, out entry)) {
                dragStartPosition = ray.GetPoint(entry);
            }
        }

        if (Input.GetMouseButton(0)) {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = GameObject.FindGameObjectWithTag("GMCamera").GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

            float entry;
            if (plane.Raycast(ray, out entry)) {
                dragCurrentPosition = ray.GetPoint(entry);
                newPosition = transform.position + dragStartPosition - dragCurrentPosition;
            }
        }

        // Drag rotate
        if (Input.GetMouseButtonDown(1)) {
            rotateStartPosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(1)) {
            rotateCurrentPosition = Input.mousePosition;
            Vector3 difference = rotateStartPosition - rotateCurrentPosition;
            rotateStartPosition = rotateCurrentPosition;
            newRotation *= Quaternion.Euler(Vector3.up * (-difference.x / rotationAmount));
        }
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
}
