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

    public void JumpForce()
    {
        playerConnector.currentPlayerState.JumpForce();
    }
}

