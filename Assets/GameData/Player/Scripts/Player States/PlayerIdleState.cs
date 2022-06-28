using UnityEngine;

public class PlayerIdleState : PlayerStates
{
    /// <summary>
    /// Constructor for the idle state.
    /// </summary>
    /// <param name="player">Reference to the player to fill required variables. </param>
    public PlayerIdleState(Player player) : base(player) { }

    /// <summary>
    /// Start function for the Idle state.
    /// </summary>
    public override void Init()
    {
        animController.SetBool(playerConnector.animCrouchBool, false);
        Movement();
    }

    /// <summary>
    /// Update function for the idle function, performs base global functional and apply gravity via character controller move.
    /// </summary>
    public override void Update()
    {
        base.Update();
        player.characterController.Move(gravityMovement);

        if (playerConnector.combatMode)
            CameraRotationMatching();
    }

    /// <summary>
    /// Movement function for the idle state which transitions to the correct state, walking running or crouchign base on the input.
    /// </summary>
    public override void Movement()
    {
        if (playerConnector.walkMode)
            ChangePlayerState(PlayerState.Walking);
        else if (playerConnector.crouchMode && playerConnector.combatMode == false)
            ChangePlayerState(PlayerState.Crouching);
        else if (playerConnector.movementRaw.magnitude >= Mathf.Epsilon)
            ChangePlayerState(PlayerState.Running);
    }

    /// <summary>
    /// Jump function to transiton into the jump state.
    /// </summary>
    public override void Jump()
    {
        ChangePlayerState(PlayerState.Jump);
    }

    /// <summary>
    /// Allows this stat to draw weapon / toggle into combat anims.
    /// </summary>
    public override void Combat()
    {
        base.Combat();
    }

    /// <summary>
    /// Idle left hand attack function
    /// </summary>
    public override void LeftHandAttack()
    {
        base.LeftHandAttack();
    }

    /// <summary>
    /// Idle right hand attack function
    /// </summary>
    public override void RightHandAttack()
    {
        base.RightHandAttack();
    }

    /// <summary>
    /// idle parry/block/dual wield attack function
    /// </summary>
    public override void ParryDualAttack()
    {
        base.ParryDualAttack();
    }

    /// <summary>
    /// available transitons for the idle state.
    /// </summary>
    /// <param name="playerstate">State to transiton into. </param>
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
            case PlayerState.Falling:
                playerConnector.playerState = PlayerState.Falling;
                playerConnector.currentPlayerState = new PlayerFallingState(player);
                Debug.Log("Change Player State to Falling");
                break;
            case PlayerState.Attack:
                playerConnector.playerState = PlayerState.Attack;
                playerConnector.currentPlayerState = new PlayerAttackState(player);
                Debug.Log("Change Player State to Attack");
                break;
            case PlayerState.Parry:
                playerConnector.playerState = PlayerState.Parry;
                playerConnector.currentPlayerState = new PlayerParryDualWeildState(player);
                Debug.Log("Change Player State to Parry");
                break;
        }
        playerConnector.currentPlayerState.Init();
    }

}
