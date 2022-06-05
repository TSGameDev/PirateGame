using UnityEngine;

[RequireComponent(typeof(InputManager))]
public class Player : MonoBehaviour
{
    #region Dependancies

    [SerializeField] PlayerConnector playerConnector;

    #endregion

    #region Movement Variables

    CharacterController characterController;
    [SerializeField] Camera playerCam;

    #endregion

    #region Animations

    Animator animController;

    int animMovementXHash = Animator.StringToHash("MovementX");
    int animMovementYHash = Animator.StringToHash("MovementY");

    #endregion

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animController = GetComponent<Animator>();
    }

    private void Update()
    {
        Movement();
        CameraRotationMatching();
    }

    private void FixedUpdate()
    {
        Gravity();
    }

    /// <summary>
    /// Movement function that takes in the raw vector2, divides it into X and Y and then assembles a Vector3 relative to the camera rotation. 
    /// If there is a movement input it will move the character controller
    /// </summary>
    public void Movement()
    {
        float x = playerConnector.movementRaw.x;
        float y = playerConnector.movementRaw.y;
        animController.SetFloat(animMovementXHash, x);
        animController.SetFloat(animMovementYHash, y);

        Vector3 movementInput = (transform.right * x) + (transform.forward * y);

        if (movementInput.magnitude >= Mathf.Epsilon)
        {
            characterController.Move(movementInput.normalized * playerConnector.speed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Function implimenting gravity to the player as the character controller component isn't effected by natural gravity
    /// </summary>
    private void Gravity()
    {
        if (!characterController.isGrounded)
        {
            characterController.Move(Physics.gravity * Time.deltaTime);
        }
    }

    /// <summary>
    /// Function that makes the player rotation match the camera rotation meaning the player will look where the camera is pointing
    /// </summary>
    private void CameraRotationMatching()
    {
        Quaternion newPlayerRot = playerCam.transform.rotation;
        newPlayerRot.x = 0;
        newPlayerRot.z = 0;
        transform.rotation = newPlayerRot;
    }
}
