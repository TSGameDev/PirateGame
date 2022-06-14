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

    #region Gravity Variables

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

    /// <summary>
    /// Function to perform all starting functionality for the state like turning animations on and others off.
    /// </summary>
    public virtual void Init() { }

    /// <summary>
    /// The base change player state function decleration which all other states rely on, needs to be in all states to stop errors even if there is no functionality for it in the called state.
    /// </summary>
    /// <param name="playerstate">The state the player needs to transition to</param>
    public virtual void ChangePlayerState(PlayerState playerstate) { }

    /// <summary>
    /// The base Update function decleration which all other states rely on, needs to be in all states to stop errors even if there is no functionality for it in the called state.
    /// </summary>
    public virtual void Update() 
    {
        Gravity();
        Movement();
        CameraRotationMatching();
    }

    /// <summary>
    ///The base movement function decleration which all other states rely on, needs to be in all states to stop errors even if there is no functionality for it in the called state. 
    /// </summary>
    public virtual void Movement() 
    {
        float speed;
        if (playerConnector.walkMode || playerConnector.crouchMode)
            speed = playerConnector.walkSpeed;
        else if (playerConnector.sprintMode)
            speed = playerConnector.sprintSpeed;
        else
            speed = playerConnector.runSpeed;

        float rawX = playerConnector.movementRaw.x;
        float rawY = playerConnector.movementRaw.y;

        animController.SetFloat(playerConnector.animMovementXHash, rawX);
        animController.SetFloat(playerConnector.animMovementYHash, rawY);

        Vector3 movementInput = (player.transform.right * rawX) + (player.transform.forward * rawY);
        movementInput = movementInput.normalized * speed * Time.deltaTime;

        if (movementInput.magnitude >= Mathf.Epsilon)
            player.characterController.Move(movementInput + gravityMovement);
        else
            ChangePlayerState(PlayerState.Idle);
    }

    /// <summary>
    /// The base camera rotation function decleration which all other states rely on, needs to be in all states to stop errors even if there is no functionality for it in the called state.
    /// </summary>
    public virtual void CameraRotationMatching() 
    {
        Quaternion newPlayerRot = player.playerCam.transform.rotation;
        newPlayerRot.x = 0;
        newPlayerRot.z = 0;
        player.transform.rotation = newPlayerRot;
    }

    /// <summary>
    /// Function that checked to make sure the player is grounded to provide gravity functionlaity.
    /// </summary>
    /// <returns>Returns a bool, true if the player is grounded, false if the player is not.</returns>
    public virtual bool IsGrounded()
    {
        return player.characterController.isGrounded;
    }

    /// <summary>
    /// Function that provides gravity functionlaity depending on if the player is grounded or not.
    /// </summary>
    public virtual void Gravity()
    {
        if(IsGrounded() && !playerConnector.jumpingTriggered)
        {
            currentGravity = playerConnector.constantGravity;
        }
        else
        {
            if(currentGravity > playerConnector.maxGravity)
            {
                currentGravity -= playerConnector.gravity * Time.deltaTime;
            }
        }

        gravityMovement = gravityDirection * -currentGravity * Time.deltaTime;
    }

    /// <summary>
    /// Transitions into jump state to perform jump activation via init function
    /// </summary>
    public virtual void Jump() 
    {
        ChangePlayerState(PlayerState.Jump);
    }

    /// <summary>
    /// Function that is triggered by anim event to applys the force of jumping to player
    /// </summary>
    public virtual void JumpForce() 
    {
        currentGravity = playerConnector.jumpForce;
    }

}

public class PlayerIdleState : PlayerStates
{
    /// <summary>
    /// The states constructor which is defined in the base PlayerStates class
    /// </summary>
    /// <param name="player">The monobehaviour player script that holds the declearation of the state machine</param>
    public PlayerIdleState(Player player) : base(player) { }

    public override void Update()
    {
        
    }

    /// <summary>
    /// Trigger by the input system to go into the walking state that provide the continous functionlaity for walking.
    /// </summary>
    public override void Movement()
    {
        if (playerConnector.walkMode)
            ChangePlayerState(PlayerState.Walking);
        else if (playerConnector.crouchMode)
            ChangePlayerState(PlayerState.Crouching);
        else
            ChangePlayerState(PlayerState.Running);
    }

    public override void Jump()
    {
        base.Jump();
    }

    /// <summary>
    /// Function that changes the player state and performs the functionality to transition into the passed in state
    /// </summary>
    /// <param name="playerstate">The State the player need to transition into</param>
    public override void ChangePlayerState(PlayerState playerstate)
    {
        switch (playerstate)
        {
            case PlayerState.Walking:
                playerConnector.playerState = PlayerState.Walking;
                playerConnector.currentPlayerState = new PlayerWalkingState(player);
                Debug.Log("Changed Player State to Walking");
                break;
            case PlayerState.Crouching:
                playerConnector.playerState = PlayerState.Crouching;
                playerConnector.currentPlayerState = new PlayerCrouchingState(player);
                Debug.Log("Change Player State to Crouching");
                break;
            case PlayerState.Running:
                playerConnector.playerState = PlayerState.Running;
                playerConnector.currentPlayerState = new PlayerRunningState(player);
                Debug.Log("Change Player State to Running");
                break;
            case PlayerState.Sprinting:
                playerConnector.playerState = PlayerState.Sprinting;
                playerConnector.currentPlayerState = new PlayerSprintingState(player);
                Debug.Log("Change Player State to Sprinting");
                break;
            case PlayerState.Jump:
                playerConnector.playerState = PlayerState.Jump;
                playerConnector.currentPlayerState = new PlayerJumpState(player);
                Debug.Log("Change Player State to Jump");
                break;
        }
        playerConnector.currentPlayerState.Init();
    }

}

public class PlayerWalkingState : PlayerStates
{
    /// <summary>
    /// The states constructor which is defined in the base PlayerStates class
    /// </summary>
    /// <param name="player">The monobehaviour player script that holds the declearation of the state machine</param>
    public PlayerWalkingState(Player player) : base(player) { }

    /// <summary>
    /// Function that perform inisilisation functionality for this state
    /// </summary>
    public override void Init()
    {
        animController.SetBool(playerConnector.animWalkBool, true);
        animController.SetBool(playerConnector.animSprintBool, false);
        animController.SetBool(playerConnector.animCrouchBool, false);
    }

    /// <summary>
    /// A function that represent the Update function of Monobehaviours by having this state function called on the monohaviour player script in the Update function.
    /// Allows for continous functionlaity from the state machine
    /// </summary>
    public override void Update()
    {
        base.Update();
    }

    /// <summary>
    /// function that provide the movement for the player by taking the raw input, creating a Vector3 and moves the character controller by that vector3 every second times speed. 
    /// Also handles the animations by changing the X and Y animation controller variables which changes the animations from idle to the correct walking animation via a blend tree.
    /// </summary>
    public override void Movement()
    {
        if (!playerConnector.walkMode)
        {
            ChangePlayerState(PlayerState.Running);
            return;
        }

        base.Movement();
    }

    public override void Jump()
    {
        base.Jump();
    }

    /// <summary>
    /// Function that changes the player state and performs the functionality to transition into the passed in state
    /// </summary>
    /// <param name="playerstate">The State the player need to transition into</param>
    public override void ChangePlayerState(PlayerState playerstate)
    {
        switch (playerstate)
        {
            case PlayerState.Idle:
                playerConnector.playerState = PlayerState.Idle;
                playerConnector.currentPlayerState = new PlayerIdleState(player);
                Debug.Log("Changed Player State to Idle");
                break;
            case PlayerState.Crouching:
                playerConnector.playerState = PlayerState.Crouching;
                playerConnector.currentPlayerState = new PlayerCrouchingState(player);
                Debug.Log("Changed Player State to Crouching");
                break;
            case PlayerState.Running:
                playerConnector.playerState = PlayerState.Running;
                playerConnector.currentPlayerState = new PlayerRunningState(player);
                Debug.Log("Changed Player State to Running");
                break;
            case PlayerState.Sprinting:
                playerConnector.playerState = PlayerState.Sprinting;
                playerConnector.currentPlayerState = new PlayerSprintingState(player);
                Debug.Log("Changed Player State to Sprinting");
                break;
            case PlayerState.Jump:
                playerConnector.playerState = PlayerState.Jump;
                playerConnector.currentPlayerState = new PlayerJumpState(player);
                Debug.Log("Change Player State to Jump");
                break;
        }
        playerConnector.currentPlayerState.Init();
    }

}

public class PlayerRunningState : PlayerStates
{
    /// <summary>
    /// Constructor for this state gather from the base blueprint class PlayerStates
    /// </summary>
    /// <param name="player">Reference to the player to fill variables</param>
    public PlayerRunningState(Player player) : base(player) { }

    /// <summary>
    /// Function that perform inisilisation functionality for this state
    /// </summary>
    public override void Init()
    {
        animController.SetBool(playerConnector.animWalkBool, false);
        animController.SetBool(playerConnector.animSprintBool, false);
        animController.SetBool(playerConnector.animCrouchBool, false);
    }

    /// <summary>
    /// Update method that is used to provide continous functionality within the state machine. Is called on the Monobehaviour player script.
    /// </summary>
    public override void Update()
    {
        base.Update();
    }

    /// <summary>
    /// Movement function that is used to provid movement functionlaity when in this state if it is applicable.
    /// </summary>
    public override void Movement()
    {
        if (playerConnector.walkMode)
        {
            ChangePlayerState(PlayerState.Walking);
            return;
        }
        else if(playerConnector.sprintMode && playerConnector.stamina > playerConnector.sprintingStaminaStartCost)
        {
            ChangePlayerState(PlayerState.Sprinting);
            return;
        }
        else if(playerConnector.crouchMode)
        {
            ChangePlayerState(PlayerState.Crouching);
            return;
        }

        base.Movement();
    }

    public override void Jump()
    {
        base.Jump();
    }

    /// <summary>
    /// Function to transition between playerstates performing the required functionlaity for each possible state. Also limits what states transition into what state.
    /// </summary>
    /// <param name="playerstate">the player state you wish to transition into</param>
    public override void ChangePlayerState(PlayerState playerstate)
    {
        switch(playerstate)
        {
            case PlayerState.Idle:
                playerConnector.playerState = PlayerState.Idle;
                playerConnector.currentPlayerState = new PlayerIdleState(player);
                Debug.Log("Changed Player State to Idle");
                break;
            case PlayerState.Walking:
                playerConnector.playerState = PlayerState.Walking;
                playerConnector.currentPlayerState = new PlayerWalkingState(player);
                Debug.Log("Changed Player State to Walking");
                break;
            case PlayerState.Crouching:
                playerConnector.playerState = PlayerState.Crouching;
                playerConnector.currentPlayerState = new PlayerCrouchingState(player);
                Debug.Log("Changed Player State to Crouching");
                break;
            case PlayerState.Sprinting:
                playerConnector.playerState = PlayerState.Sprinting;
                playerConnector.currentPlayerState = new PlayerSprintingState(player);
                Debug.Log("Changed Player State to Sprinting");
                break;
            case PlayerState.Jump:
                playerConnector.playerState = PlayerState.Jump;
                playerConnector.currentPlayerState = new PlayerJumpState(player);
                Debug.Log("Change Player State to Jump");
                break;
        }
        playerConnector.currentPlayerState.Init();
    }

}

public class PlayerSprintingState : PlayerStates
{
    /// <summary>
    /// Constructor for this state gather from the base blueprint class PlayerStates
    /// </summary>
    /// <param name="player">Reference to the player to fill variables</param>
    public PlayerSprintingState(Player player) : base(player) { }

    /// <summary>
    /// Function that perform inisilisation functionality for this state
    /// </summary>
    public override void Init()
    {
        animController.SetBool(playerConnector.animWalkBool, false);
        animController.SetBool(playerConnector.animSprintBool, true);
        animController.SetBool(playerConnector.animCrouchBool, false);
    }

    /// <summary>
    /// Update method that is used to provide continous functionality within the state machine. Is called on the Monobehaviour player script.
    /// </summary>
    public override void Update()
    {
        base.Update();
    }

    /// <summary>
    /// Movement function that is used to provid movement functionlaity when in this state if it is applicable.
    /// </summary>
    public override void Movement()
    {
        if (playerConnector.walkMode)
        {
            ChangePlayerState(PlayerState.Walking);
            return;
        }
        else if (!playerConnector.sprintMode && playerConnector.stamina > Mathf.Epsilon)
        {
            ChangePlayerState(PlayerState.Running);
            return;
        }

        base.Movement();

        playerConnector.stamina =- playerConnector.sprintingStaminaStartCost * Time.deltaTime;
    }

    public override void Jump()
    {
        base.Jump();
    }

    /// <summary>
    /// Function to transition between playerstates performing the required functionlaity for each possible state. Also limits what states transition into what state.
    /// </summary>
    /// <param name="playerstate">the player state you wish to transition into</param>
    public override void ChangePlayerState(PlayerState playerstate)
    {
        switch(playerstate)
        {
            case PlayerState.Idle:
                playerConnector.playerState = PlayerState.Idle;
                playerConnector.currentPlayerState = new PlayerIdleState(player);
                Debug.Log("Changed Player State to Idle");
                break;
            case PlayerState.Running:
                playerConnector.playerState = PlayerState.Running;
                playerConnector.currentPlayerState = new PlayerRunningState(player);
                Debug.Log("Changed Player State to Running");
                break;
            case PlayerState.Walking:
                playerConnector.playerState = PlayerState.Walking;
                playerConnector.currentPlayerState = new PlayerWalkingState(player);
                Debug.Log("Changed Player State to Sprinting");
                break;
            case PlayerState.Jump:
                playerConnector.playerState = PlayerState.Jump;
                playerConnector.currentPlayerState = new PlayerJumpState(player);
                Debug.Log("Change Player State to Jump");
                break;
        }
        playerConnector.currentPlayerState.Init();
    }

}

public class PlayerCrouchingState : PlayerStates
{
    /// <summary>
    /// Constructor for this state gather from the base blueprint class PlayerStates
    /// </summary>
    /// <param name="player">Reference to the player to fill variables</param>
    public PlayerCrouchingState(Player player) : base(player) { }

    /// <summary>
    /// Function that perform inisilisation functionality for this state
    /// </summary>
    public override void Init()
    {
        animController.SetBool(playerConnector.animWalkBool, false);
        animController.SetBool(playerConnector.animSprintBool, false);
        animController.SetBool(playerConnector.animCrouchBool, true);
    }

    /// <summary>
    /// Update method that is used to provide continous functionality within the state machine. Is called on the Monobehaviour player script.
    /// </summary>
    public override void Update()
    {
        base.Update();
    }

    /// <summary>
    /// Function to transition between playerstates performing the required functionlaity for each possible state. Also limits what states transition into what state.
    /// </summary>
    /// <param name="playerstate">the player state you wish to transition into</param>
    public override void ChangePlayerState(PlayerState playerstate)
    {
        switch(playerstate)
        {
            case PlayerState.Idle:
                playerConnector.playerState = PlayerState.Idle;
                playerConnector.currentPlayerState = new PlayerIdleState(player);
                Debug.Log("Changed Player State to Idle");
                break;
            case PlayerState.Walking:
                playerConnector.playerState = PlayerState.Walking;
                playerConnector.currentPlayerState = new PlayerWalkingState(player);
                Debug.Log("Changed Player State to Walking");
                break;
            case PlayerState.Running:
                playerConnector.playerState = PlayerState.Running;
                playerConnector.currentPlayerState = new PlayerRunningState(player);
                Debug.Log("Changed Player State to Running");
                break;
        }
        playerConnector.currentPlayerState.Init();
    }

}

public class PlayerJumpState : PlayerStates
{
    public PlayerJumpState(Player player) : base(player) { }

    public override void Init()
    {
        if (playerConnector.jumpingTriggered)
            return;


        animController.SetTrigger(playerConnector.animJumpHash);
        playerConnector.jumpingTriggered = true;
    }

    public override void JumpForce()
    {
        base.JumpForce();
        Debug.Log("Jump Force Called");
    }

}

public class PlayerFallingState : PlayerStates
{
    public PlayerFallingState(Player player) : base(player) { }

    public override void Init()
    {
        animController.SetTrigger(playerConnector.animFallingHash);
    }
}