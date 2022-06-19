using UnityEngine;

public enum PlayerState
{
    Idle,
    Walking,
    Running,
    Sprinting,
    Crouching,
    Jump,
    Falling
}

public abstract class PlayerStates
{
    #region Dependencies
    
    // All the variables required in all the states making them accessable no matter what state is currently active.
    protected Player player;
    protected PlayerConnector playerConnector;
    protected Animator animController;

    #endregion

    #region Movement Variables

    //Global movement variable to allow other functions to know if there is input or not. Required for determining the landing animation in the falling function.
    protected Vector3 movement;

    #endregion

    #region Gravity Variables

    //Variables that are used to impliment and store gravity related functionlaity
    protected float currentGravity;
    protected Vector3 gravityDirection;
    protected Vector3 gravityMovement;

    #endregion

    /// <summary>
    /// The base constructor that fills the required variables that all other state constructors rely on.
    /// </summary>
    /// <param name="player">The monobehaviour player script that contains the deceration of this state machine</param>
    public PlayerStates(Player player)
    {
        this.player = player;
        playerConnector = player.playerConnector;
        animController = player.animController;

        gravityDirection = Vector3.down;
    }

    #region State Dependant Functionlaity

    /// <summary>
    /// A template function used by the states to impliment start up functionlaity.
    /// </summary>
    public virtual void Init() { }

    /// <summary>
    /// A template function used by the state to transition from one state to another. After the transition has compelte, this function also calls the start up function Init in the new state.
    /// </summary>
    /// <param name="playerstate">The state to transition into</param>
    public virtual void ChangePlayerState(PlayerState playerstate) { }

    /// <summary>
    /// A template function used by the states to impliment continous functionlaity. The function is called in the Update function of the Monobehaviour player script.
    /// Contains the global functionlaity of Gravity and Falling functions.
    /// </summary>
    public virtual void Update() 
    {
        Gravity();
        Falling();
    }

    /// <summary>
    /// A template fucntion used by the states to impliment movement functionlaity.
    /// </summary>
    public virtual void Movement() { }

    /// <summary>
    /// A template function used by the states to impliment the functionlity for matching the character rotation to the camera rotation.
    /// This function is defined in the base template but some state don't use the function so it is state dependant.
    /// </summary>
    public virtual void CameraRotationMatching()
    {
        Quaternion newPlayerRot = player.playerCam.transform.rotation;
        newPlayerRot.x = 0;
        newPlayerRot.z = 0;
        player.transform.rotation = newPlayerRot;
    }

    /// <summary>
    /// A template function used by the states to transition into the jump state.
    /// </summary>
    public virtual void Jump() { }

    /// <summary>
    /// A template function used by an animation event to impliment the jump force on the correct keyframe. Called by the Public function on the monobehaviour player script via an animation event.
    /// </summary>
    public virtual void JumpForce() { }

    #endregion

    #region Global Functionality

    /// <summary>
    /// A global function used to check if the player is currently grounded. Makes use of the character controller.
    /// </summary>
    /// <returns> Returns a bool, True for grounded, false for not grounded.</returns>
    public virtual bool IsGrounded()
    {
        return player.characterController.isGrounded;
    }

    /// <summary>
    /// A global function used to check if the player is currently falling.
    /// </summary>
    /// <returns> Returns a bool, true if the character is falling or false if they are not</returns>
    public virtual bool IsFalling()
    {
        if (playerConnector.fallingSpeed < playerConnector.fallingThresHold)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// A global function used to impliment gravity onto the character.
    /// </summary>
    public virtual void Gravity()
    {
        if (IsGrounded() && !playerConnector.jumpingTriggered)
        {
            currentGravity = playerConnector.constantGravity;
        }
        else
        {
            if (currentGravity > playerConnector.maxGravity)
            {
                currentGravity -= playerConnector.gravity * Time.deltaTime;
            }
        }

        gravityMovement = gravityDirection * -currentGravity * Time.deltaTime;
    }

    /// <summary>
    /// A global function that impliments that state changing for falling and landing.
    /// </summary>
    public virtual void Falling()
    {
        playerConnector.fallingSpeed = player.transform.InverseTransformDirection(player.characterController.velocity).y;

        if (IsFalling() && !IsGrounded() && !playerConnector.jumpingTriggered && !playerConnector.fallingTriggered)
        {
            ChangePlayerState(PlayerState.Falling);
        }

        if (playerConnector.fallingTriggered && IsGrounded() && playerConnector.fallingSpeed < -0.1f)
        {
            playerConnector.fallingTriggered = false;
            playerConnector.jumpingTriggered = false;

            if (movement.magnitude > Mathf.Epsilon)
                ChangePlayerState(PlayerState.Running);
            else
                ChangePlayerState(PlayerState.Idle);
        }
    }

    #endregion

}