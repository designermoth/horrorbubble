using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public Camera playerCamera;
    //public float walkSpeed = 6f;
    public float runSpeed = 0f;
    //public float gravity = 10f;

    private float currSpeedX = 0f;
    private float currSpeedY = 0f;


    public float lookSpeed = 0f;
    public float lookXlimit = 0f;

    Vector3 moveDirection = Vector3.zero;
    Vector3 forward;
    Vector3 right;
    float rotationX = 0f;

    public bool canMove = true;

    CharacterController characterController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        #region Handles Movement
        forward = transform.TransformDirection(Vector3.forward);
        right = transform.TransformDirection(Vector3.right);

        currSpeedX = canMove ? (runSpeed) * Input.GetAxis("Vertical") : 0;
        currSpeedY = canMove ? (runSpeed) * Input.GetAxis("Horizontal") : 0;
        moveDirection = (forward * currSpeedX) + (right * currSpeedY);

        #endregion


    }
    void Update()
    {
        #region Rotation
        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXlimit, lookXlimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);

        }
        #endregion
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            SceneManager.LoadScene(3);
        }
        if (collision.gameObject.CompareTag("Win"))
        {
            SceneManager.LoadScene(2);
        }
    }
}
