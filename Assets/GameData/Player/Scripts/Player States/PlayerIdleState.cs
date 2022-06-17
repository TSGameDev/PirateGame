using UnityEngine;

public class PlayerIdleState : PlayerStates
{
    /// <summary>
    /// The states constructor which is defined in the base PlayerStates class
    /// </summary>
    /// <param name="player">The monobehaviour player script that holds the declearation of the state machine</param>
    public PlayerIdleState(Player player) : base(player) { }

    public override void Update()
    {
        Gravity();
        Falling();
        player.characterController.Move(gravityMovement);
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

    public override void Gravity()
    {
        base.Gravity();
    }

    public override void Falling()
    {
        base.Falling();
    }

    public override void Jump()
    {
        ChangePlayerState(PlayerState.Jump);
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
            case PlayerState.Falling:
                playerConnector.playerState = PlayerState.Falling;
                playerConnector.currentPlayerState = new PlayerFallingState(player);
                Debug.Log("Change Player State to Falling");
                break;

        }
        playerConnector.currentPlayerState.Init();
    }

}
