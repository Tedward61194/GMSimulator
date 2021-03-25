using UnityEngine;
using Mirror;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float attackCooldown;
    public float attackRange;
    public float attackDamage;

    [SerializeField] Transform cameraTransform;
    [SerializeField] float mouseSensitivity;
    [SerializeField] float walkSpeed;
    [SerializeField][Range(0.0f, 0.05f)] float moveSmoothTime;
    [SerializeField] float jumpForce;
    [SerializeField] float gravity;
    [SerializeField] Transform attackPoint;
    [SerializeField] LayerMask enemyLayer;

    bool paused;
    bool canRun = true;
    bool canAttack = true;
    float cameraPitch = 0.0f;
    float velocityY;
    CharacterController controller;
    PaladinAnimationStateController animationController;
    Vector2 currentDir = Vector2.zero;
    Vector2 currentDirVelocity = Vector2.zero;

    void Start() {
        controller = GetComponent<CharacterController>();
        cameraTransform = GetComponentInChildren<Camera>().transform;
        animationController = GetComponent<PaladinAnimationStateController>();

        // Only allow client camera to be active
        if (!GetComponentInParent<NetworkIdentity>().isLocalPlayer) {
            cameraTransform.gameObject.SetActive(false);
        } else {
            // Hide glasses
            //This name may change later/might not be applicable.
            //Should probablly be a list of objects to hide or a layer or something but it's fine for now.
            //transform.Find("Player1Body").transform.Find("Sunglasses").gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (GetComponentInParent<NetworkIdentity>().isLocalPlayer) {
            ListenForPause();
            if (!paused) {
                UpdateMouseLook();
                UpdateMovement();
                ListenForAttack();
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
        if (canRun) {
            Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            targetDir.Normalize(); // Needed to stop diagonal vector from moving at hypotenuse' magnitude
            currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);
            if (currentDir.normalized.magnitude > 0) {
                animationController.SetIsRunning(true);
            } else {
                animationController.SetIsRunning(false);
            }
        }

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

    private void ListenForAttack() {
        if (canAttack) {
            if (Input.GetMouseButtonDown(0)) {
                Attack();
                StartCoroutine("AttackCooldown");
            }
        }
    }

    private void Attack() {
        animationController.AttackOne();
        Collider[] enemiesHit = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider enemy in enemiesHit) {
            enemy.GetComponent<EnemyManageger>().ApplyDamage(attackDamage);
        }

    }
    public IEnumerator AttackCooldown() {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}
