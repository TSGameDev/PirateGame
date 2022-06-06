using UnityEngine;

public enum PlayerState
{
    Idle,
    Walking
}

public abstract class PlayerStates
{
    #region Dependencies

    protected Player player;
    protected PlayerConnector playerConnector;
    protected Animator animController;
    #endregion

    public PlayerStates(Player player)
    {
        this.player = player;
        playerConnector = player.playerConnector;
        animController = player.animController;
    }

    public virtual void ChangePlayerState(PlayerState playerstate) { }

    public virtual void Update() { }

    public virtual void Movement() { }

}

public class PlayerIdleState : PlayerStates
{
    public PlayerIdleState(Player player) : base(player) { }

    public override void Movement()
    {
        ChangePlayerState(PlayerState.Walking);
    }

    public override void ChangePlayerState(PlayerState playerstate)
    {
        switch (playerstate)
        {
            case PlayerState.Walking:
                playerConnector.playerState = PlayerState.Walking;
                playerConnector.currentPlayerState = new PlayerWalkingState(player);
                Debug.Log("Changed Player State to Walking");
                break;
        }
    }

}

public class PlayerWalkingState : PlayerStates
{
    public PlayerWalkingState(Player player) : base(player) { }

    public override void Update()
    {
        Movement();
    }

    public override void Movement()
    {
        float x = playerConnector.movementRaw.x;
        float y = playerConnector.movementRaw.y;
        animController.SetFloat(playerConnector.animMovementXHash, x);
        animController.SetFloat(playerConnector.animMovementYHash, y);

        Vector3 movementInput = (player.transform.right * x) + (player.transform.forward * y);

        if (movementInput.magnitude >= Mathf.Epsilon)
            player.characterController.Move(movementInput.normalized * playerConnector.speed * Time.deltaTime);
        else
            ChangePlayerState(PlayerState.Idle);
    }

    public override void ChangePlayerState(PlayerState playerstate)
    {
        switch (playerstate)
        {
            case PlayerState.Idle:
                playerConnector.playerState = PlayerState.Idle;
                playerConnector.currentPlayerState = new PlayerIdleState(player);
                Debug.Log("Changed Player State to Idle");
                break;
        }
    }

}
