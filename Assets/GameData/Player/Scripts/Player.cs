using UnityEngine;

[RequireComponent(typeof(InputManager))]
public class Player : MonoBehaviour
{
    #region Dependancies

    public PlayerConnector playerConnector;

    #endregion

    #region Movement Variables

    public CharacterController characterController;
    public Camera playerCam;

    #endregion

    #region Animations

    public Animator animController;

    #endregion

    private void Awake()
    {
        playerConnector.currentPlayerState = new PlayerIdleState(this);
        characterController = GetComponent<CharacterController>();
        animController = GetComponent<Animator>();
    }

    private void Update()
    {
        playerConnector.currentPlayerState.Update();
    }

    private void FixedUpdate()
    {
        Gravity();
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

}
