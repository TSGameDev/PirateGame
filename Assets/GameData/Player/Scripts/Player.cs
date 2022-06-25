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

    /// <summary>
    /// Function called when the application starts up before Start. creates and/or defines variables.
    /// </summary>
    private void Awake()
    {
        playerConnector.currentPlayerState = new PlayerIdleState(this);
        characterController = GetComponent<CharacterController>();
        animController = GetComponent<Animator>();
    }

    /// <summary>
    /// Update function ran every frame.
    /// </summary>
    private void Update()
    {
        playerConnector.currentPlayerState.Update();
    }

    /// <summary>
    /// Public function that is called by an anim event to calls the state machine jump force function.
    /// </summary>
    public void JumpForce()
    {
        playerConnector.currentPlayerState.JumpForce();
    }

    /// <summary>
    /// Public function called by an anim event to transition the state mahcine into the falling state after ending the jump.
    /// </summary>
    public void JumpFallingTransition()
    {
        playerConnector.currentPlayerState.ChangePlayerState(PlayerState.Falling);
    }

    public void ComboSetter()
    {
        playerConnector.comboPossible = !playerConnector.comboPossible;
    }

    public void ComboResetter()
    {
        playerConnector.comboPossible = false;
        playerConnector.comboStep = 0;
    }
}

