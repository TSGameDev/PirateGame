using UnityEngine;

public class PlayerRunningState : PlayerStates
{
    /// <summary>
    /// The constructor for the running state.
    /// </summary>
    /// <param name="player">A reference to the player to fill required vairables</param>
    public PlayerRunningState(Player player) : base(player) { }

    /// <summary>
    /// The start function for the running state, sets anims to running
    /// </summary>
    public override void Init()
    {
        animController.SetBool(playerConnector.animWalkBool, false);
        animController.SetBool(playerConnector.animSprintBool, false);
        animController.SetBool(playerConnector.animCrouchBool, false);
    }

    /// <summary>
    /// Update function for the running state, performs base global functionality, movement and camera rotation matching.
    /// </summary>
    public override void Update()
    {
        base.Update();
        Movement();
        CameraRotationMatching();
    }

    /// <summary>
    /// The movement function for the running state, sets XY anim values, player character movement and gravity.
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
        else if (playerConnector.crouchMode && !playerConnector.combatMode)
        {
            ChangePlayerState(PlayerState.Crouching);
            return;
        }

        float rawX = playerConnector.movementRaw.x;
        float rawY = playerConnector.movementRaw.y;

        animController.SetFloat(playerConnector.animMovementXHash, rawX, 0.1f, Time.deltaTime);
        animController.SetFloat(playerConnector.animMovementYHash, rawY, 0.1f, Time.deltaTime);

        movement = (player.transform.right * rawX) + (player.transform.forward * rawY);
        movement = movement.normalized * playerConnector.runSpeed * Time.deltaTime;

        if (movement.magnitude >= Mathf.Epsilon)
            player.characterController.Move(movement + gravityMovement);
        else
            ChangePlayerState(PlayerState.Idle);
    }

    /// <summary>
    /// The jump function that transitions to the jump state.
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
    /// Running State left hand attack function
    /// </summary>
    public override void LeftHandAttack()
    {
        base.LeftHandAttack();
    }

    /// <summary>
    /// Running State right hand attack function
    /// </summary>
    public override void RightHandAttack()
    {
        base.RightHandAttack();
    }

    /// <summary>
    /// Running State parry/block/dual wield attack function
    /// </summary>
    public override void ParryDualAttack()
    {
        base.ParryDualAttack();
    }

    /// <summary>
    /// Transitions available from the running state.
    /// </summary>
    /// <param name="playerstate">State to transition into.</param>
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
            case PlayerState.Attack:
                playerConnector.playerState = PlayerState.Attack;
                playerConnector.currentPlayerState = new PlayerAttackState(player);
                Debug.Log("Change Player State to Attack");
                break;
            case PlayerState.Parry:
                playerConnector.playerState = PlayerState.Parry;
                playerConnector.currentPlayerState = new PlayerParryState(player);
                Debug.Log("Change Player State to Parry");
                break;
        }
        playerConnector.currentPlayerState.Init();
    }
}
