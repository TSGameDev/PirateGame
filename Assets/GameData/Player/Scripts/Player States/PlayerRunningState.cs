using UnityEngine;

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
        Gravity();
        Falling();
        Movement();
        CameraRotationMatching();
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
        else if (playerConnector.sprintMode && playerConnector.stamina > playerConnector.sprintingStaminaStartCost)
        {
            ChangePlayerState(PlayerState.Sprinting);
            return;
        }
        else if (playerConnector.crouchMode)
        {
            ChangePlayerState(PlayerState.Crouching);
            return;
        }

        float rawX = playerConnector.movementRaw.x;
        float rawY = playerConnector.movementRaw.y;

        animController.SetFloat(playerConnector.animMovementXHash, rawX);
        animController.SetFloat(playerConnector.animMovementYHash, rawY);

        movement = (player.transform.right * rawX) + (player.transform.forward * rawY);
        movement = movement.normalized * playerConnector.runSpeed * Time.deltaTime;

        if (movement.magnitude >= Mathf.Epsilon)
            player.characterController.Move(movement + gravityMovement);
        else
            ChangePlayerState(PlayerState.Idle);
    }

    public override void CameraRotationMatching()
    {
        base.CameraRotationMatching();
    }

    public override void Jump()
    {
        ChangePlayerState(PlayerState.Jump);
    }

    /// <summary>
    /// Function to transition between playerstates performing the required functionlaity for each possible state. Also limits what states transition into what state.
    /// </summary>
    /// <param name="playerstate">the player state you wish to transition into</param>
    public override void ChangePlayerState(PlayerState playerstate)
    {
        switch (playerstate)
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
            case PlayerState.Falling:
                playerConnector.playerState = PlayerState.Falling;
                playerConnector.currentPlayerState = new PlayerFallingState(player);
                Debug.Log("Change Player State to Falling");
                break;
        }
        playerConnector.currentPlayerState.Init();
    }

}
