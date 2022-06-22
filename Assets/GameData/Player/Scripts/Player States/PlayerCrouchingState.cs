using UnityEngine;

public class PlayerCrouchingState : PlayerStates
{
    /// <summary>
    /// Constructor for the crouching state.
    /// </summary>
    /// <param name="player">reference to the player to fill the required variables.</param>
    public PlayerCrouchingState(Player player) : base(player) { }

    /// <summary>
    /// The start function to the crouching state. Sets the anims to crouch anims.
    /// </summary>
    public override void Init()
    {
        animController.SetBool(playerConnector.animWalkBool, false);
        animController.SetBool(playerConnector.animSprintBool, false);
        animController.SetBool(playerConnector.animCrouchBool, true);
    }

    /// <summary>
    /// The update function for the crouching state.
    /// </summary>
    public override void Update()
    {
        base.Update();
        Movement();
        CameraRotationMatching();
    }

    /// <summary>
    /// The movement functionality for the crouching state. movements at walking pace.
    /// </summary>
    public override void Movement()
    {
        if (!playerConnector.crouchMode)
            ChangePlayerState(PlayerState.Idle);

        float rawX = playerConnector.movementRaw.x;
        float rawY = playerConnector.movementRaw.y;

        animController.SetFloat(playerConnector.animMovementXHash, rawX);
        animController.SetFloat(playerConnector.animMovementYHash, rawY);

        movement = (player.transform.right * rawX) + (player.transform.forward * rawY);
        movement = movement.normalized * playerConnector.walkSpeed * Time.deltaTime;

        if (movement.magnitude >= Mathf.Epsilon)
            player.characterController.Move(movement + gravityMovement);
    }

    /// <summary>
    /// Can't draw weapons when in crouching due to no crouching weapon holding anims.
    /// </summary>
    public override void Combat()
    {
        Debug.Log("Can't draw weapons when crouching");
    }

    /// <summary>
    /// The transitions available for the crouching state.
    /// </summary>
    /// <param name="playerstate">the state to transition into.</param>
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
            case PlayerState.Running:
                playerConnector.playerState = PlayerState.Running;
                playerConnector.currentPlayerState = new PlayerRunningState(player);
                Debug.Log("Changed Player State to Running");
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
