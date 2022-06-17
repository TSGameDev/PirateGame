using UnityEngine;

public class PlayerWalkingState : PlayerStates
{
    /// <summary>
    /// The constructor for the walking state, uses the base defined template.
    /// </summary>
    /// <param name="player">Reference to the player to fill required variables</param>
    public PlayerWalkingState(Player player) : base(player) { }

    /// <summary>
    /// The start functionality for the walking state, makes sure the walking animations are playing over anything else.
    /// </summary>
    public override void Init()
    {
        animController.SetBool(playerConnector.animWalkBool, true);
        animController.SetBool(playerConnector.animSprintBool, false);
        animController.SetBool(playerConnector.animCrouchBool, false);
    }

    /// <summary>
    /// The continous functionlaity for the walking state, the global functionlaity, movement and camera rotation matching.
    /// </summary>
    public override void Update()
    {
        base.Update();
        Movement();
        CameraRotationMatching();
    }

    /// <summary>
    /// The movement functionality for the walking state. Handles noticing the conditions for transitions to other states, 
    /// transfer raw inputs to calculated movement using charater controller, impliments gravity onto movement and controls X and Y anim variables for blend tree animation.
    /// </summary>
    public override void Movement()
    {
        if (!playerConnector.walkMode)
        {
            ChangePlayerState(PlayerState.Running);
            return;
        }

        float rawX = playerConnector.movementRaw.x;
        float rawY = playerConnector.movementRaw.y;

        animController.SetFloat(playerConnector.animMovementXHash, rawX);
        animController.SetFloat(playerConnector.animMovementYHash, rawY);

        movement = (player.transform.right * rawX) + (player.transform.forward * rawY);
        movement = movement.normalized * playerConnector.walkSpeed * Time.deltaTime;

        if (movement.magnitude >= Mathf.Epsilon)
            player.characterController.Move(movement + gravityMovement);
        else
            ChangePlayerState(PlayerState.Idle);
    }

    /// <summary>
    /// The jump function allowing the ability to jump from the walking state.
    /// </summary>
    public override void Jump()
    {
        ChangePlayerState(PlayerState.Jump);
    }

    /// <summary>
    /// The transitions available from the walking state.
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
            case PlayerState.Falling:
                playerConnector.playerState = PlayerState.Falling;
                playerConnector.currentPlayerState = new PlayerFallingState(player);
                Debug.Log("Change Player State to Falling");
                break;
        }
        playerConnector.currentPlayerState.Init();
    }

}
