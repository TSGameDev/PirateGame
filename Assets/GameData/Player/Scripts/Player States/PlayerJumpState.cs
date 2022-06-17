using UnityEngine;

public class PlayerJumpState : PlayerStates
{
    /// <summary>
    /// Constructor for the jump state, uses the base defined template.
    /// </summary>
    /// <param name="player">Reference to the player to fill require variables</param>
    public PlayerJumpState(Player player) : base(player) { }

    /// <summary>
    /// The start functionality for the jump state, toggle the jump triggered bool and begins the jump animations.
    /// </summary>
    public override void Init()
    {
        animController.SetTrigger(playerConnector.animJumpHash);
        playerConnector.jumpingTriggered = true;
        playerConnector.fallingTriggered = true;
    }

    /// <summary>
    /// The continous functionality for the jump state, Doesn't use the base Update global functionlaity as jumping doesn't require the falling function.
    /// instead independatnly called Gravity, Movement and Camera Rotaion Matching.
    /// </summary>
    public override void Update()
    {
        Gravity();
        Movement();
        CameraRotationMatching();
    }


    public override void Movement()
    {
        float rawX = playerConnector.movementRaw.x;
        float rawY = playerConnector.movementRaw.y;

        animController.SetFloat(playerConnector.animMovementXHash, rawX);
        animController.SetFloat(playerConnector.animMovementYHash, rawY);

        movement = (player.transform.right * rawX) + (player.transform.forward * rawY);
        movement = movement.normalized * playerConnector.walkSpeed * Time.deltaTime;

        if (movement.magnitude >= Mathf.Epsilon)
            player.characterController.Move(movement + gravityMovement);
        else
            player.characterController.Move(gravityMovement);
    }


    public override void JumpForce()
    {
        currentGravity = playerConnector.jumpForce;
    }


    public override void ChangePlayerState(PlayerState playerstate)
    {
        switch (playerstate)
        {
            case PlayerState.Falling:
                playerConnector.playerState = PlayerState.Falling;
                playerConnector.currentPlayerState = new PlayerFallingState(player);
                Debug.Log("Change Player State to Falling");
                return;
        }
        playerConnector.currentPlayerState.Init();
    }
}