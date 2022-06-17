using UnityEngine;

public class PlayerFallingState : PlayerStates
{
    /// <summary>
    /// The constructor for the falling state, uses the defined base template.
    /// </summary>
    /// <param name="player">A reference to the player to fill the require variables</param>
    public PlayerFallingState(Player player) : base(player) { }

    /// <summary>
    /// The start functionlaity for the falling state. Makes the jump and fall triggered true to stop repeated calls of falling function and any jump functions. Triggeres the falling anim.
    /// </summary>
    public override void Init()
    {
        playerConnector.fallingTriggered = true;
        playerConnector.jumpingTriggered = true;
        animController.SetTrigger(playerConnector.animFallingHash);
    }

    /// <summary>
    /// The continuous functionlaity for the falling state.
    /// </summary>
    public override void Update()
    {
        base.Update();
        Movement();
        CameraRotationMatching();
    }

    /// <summary>
    /// The movement functionlaity for the falling state. Allows for the character to move at the pace of walking in the air alongside applying gravity.
    /// </summary>
    public override void Movement()
    {
        float rawX = playerConnector.movementRaw.x;
        float rawY = playerConnector.movementRaw.y;

        movement = (player.transform.right * rawX) + (player.transform.forward * rawY);
        movement = movement.normalized * playerConnector.walkSpeed * Time.deltaTime;

        if (movement.magnitude >= Mathf.Epsilon)
            player.characterController.Move(movement + gravityMovement);
        else
            player.characterController.Move(gravityMovement);
    }

    /// <summary>
    /// The transitions avaliable from the Falling state triggered by the falling function when landed to transition into either running if there is a movement input or the idle state, 
    /// calling the correct landing anim from this decision.
    /// </summary>
    /// <param name="playerstate">The state to transition to</param>
    public override void ChangePlayerState(PlayerState playerstate)
    {
        switch (playerstate)
        {
            case PlayerState.Idle:
                playerConnector.playerState = PlayerState.Idle;
                playerConnector.currentPlayerState = new PlayerIdleState(player);
                animController.SetTrigger(playerConnector.animLandHash);
                Debug.Log("Change Player State to Idle");
                break;
            case PlayerState.Running:
                playerConnector.playerState = PlayerState.Running;
                playerConnector.currentPlayerState = new PlayerRunningState(player);
                animController.SetTrigger(playerConnector.animLandRollRun);
                Debug.Log("Change Player State to Idle");
                break;
        }
    }
}
