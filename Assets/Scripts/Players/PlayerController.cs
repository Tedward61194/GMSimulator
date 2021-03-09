using UnityEngine;
using Mirror;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform cameraTransform;
    [SerializeField] float mouseSensitivity;
    [SerializeField] float walkSpeed;
    [SerializeField][Range(0.0f, 0.05f)] float moveSmoothTime;
    [SerializeField] float jumpForce;
    [SerializeField] float gravity;

    bool paused;
    float cameraPitch = 0.0f;
    float velocityY;
    CharacterController controller;

    Vector2 currentDir = Vector2.zero;
    Vector2 currentDirVelocity = Vector2.zero;

    void Start() {
        controller = GetComponent<CharacterController>();
        cameraTransform = GetComponentInChildren<Camera>().transform;

        // Only allow client camera to be active
        if (!GetComponentInParent<NetworkIdentity>().isLocalPlayer) {
            cameraTransform.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (GetComponentInParent<NetworkIdentity>().isLocalPlayer) {
            ListenForPause();
            if (!paused) {
                UpdateMouseLook();
                UpdateMovement();
            }
        }
    }

    void UpdateMouseLook() {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        cameraPitch += -mouseDelta.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);
        cameraTransform.localEulerAngles = Vector3.right * cameraPitch;

        transform.Rotate(Vector3.up * mouseDelta.x * mouseSensitivity);
    }

    private void UpdateMovement() {
        // Inputs
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize(); // Needed to stop diagonal vector from moving at hypotenuse' magnitude
        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

        // Gravity
        if (controller.isGrounded) {
            velocityY = 0.0f;
            //Jump
            if (Input.GetKeyDown(KeyCode.Space)) {
                velocityY += jumpForce;
            }
        }
        velocityY += gravity * Time.deltaTime;

        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * walkSpeed + Vector3.up * velocityY;
        controller.Move(velocity * Time.deltaTime);
    }

    private void ListenForPause() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            paused = !paused;
        }
    }
}
