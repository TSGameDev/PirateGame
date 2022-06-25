using UnityEngine;

public class PlayerSprintingState : PlayerStates
{
    /// <summary>
    /// Constructor for the sprinting state.
    /// </summary>
    /// <param name="player">player reference to fill required variables.</param>
    public PlayerSprintingState(Player player) : base(player) { }

    /// <summary>
    /// The start function for the sprinting state, sets the anims to running.
    /// </summary>
    public override void Init()
    {
        animController.SetBool(playerConnector.animWalkBool, false);
        animController.SetBool(playerConnector.animSprintBool, true);
        animController.SetBool(playerConnector.animCrouchBool, false);
    }

    /// <summary>
    /// The update function for the sprinting state.
    /// </summary>
    public override void Update()
    {
        base.Update();
        Movement();
        CameraRotationMatching();
    }

    /// <summary>
    /// The movement fucntionality for the sprinting state. sets anim XY values, performs character controller movement with run speed.
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

        float rawX = playerConnector.movementRaw.x;
        float rawY = playerConnector.movementRaw.y;

        animController.SetFloat(playerConnector.animMovementXHash, rawX);
        animController.SetFloat(playerConnector.animMovementYHash, rawY);

        movement = (player.transform.right * rawX) + (player.transform.forward * rawY);
        movement = movement.normalized * playerConnector.sprintSpeed * Time.deltaTime;

        if (movement.magnitude >= Mathf.Epsilon)
            player.characterController.Move(movement + gravityMovement);
        else
            ChangePlayerState(PlayerState.Idle);

        //playerConnector.stamina = -playerConnector.sprintingCost * Time.deltaTime;
    }

    /// <summary>
    /// Jump function to transition into the jump state.
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
    /// The transitions available for the sprinting state.
    /// </summary>
    /// <param name="playerstate">The state to transition into.</param>
    public override void ChangePlayerState(PlayerState playerstate)
    {
        switch (playerstate)
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
            case PlayerState.Falling:
                playerConnector.playerState = PlayerState.Falling;
                playerConnector.currentPlayerState = new PlayerFallingState(player);
                Debug.Log("Change Player State to Falling");
                break;
        }
        playerConnector.currentPlayerState.Init();
    }


    /// <summary>
    /// Sprinting state left hand attack function
    /// </summary>
    public override void LeftHandAttack()
    {

    }

    /// <summary>
    /// Sprinting state right hand attack function
    /// </summary>
    public override void RightHandAttack()
    {
        if (playerConnector.movementRaw != new Vector2(0, 1))
            return;

        Debug.Log(playerConnector.movementRaw);
        animController.SetTrigger(playerConnector.animRightHandRunningAttack);
        playerConnector.combatMode = true;
    }

    /// <summary>
    /// Sprinting state parry/block/dual wield attack function
    /// </summary>
    public override void ParryDualAttack()
    {

    }

}
