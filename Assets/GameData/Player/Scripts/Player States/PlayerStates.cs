using UnityEngine;

public enum PlayerState
{
    Idle,
    Walking
}

public abstract class PlayerStates
{
    #region Dependencies
    
    // All the variables required in all the states making them accessable no matter what state is currently active.
    protected Player player;
    protected PlayerConnector playerConnector;
    protected Animator animController;

    #endregion

    /// <summary>
    /// The base constructor that fills the required variables that all other state constructors rely on.
    /// </summary>
    /// <param name="player">The monobehaviour player script that contains the deceration of this state machine</param>
    public PlayerStates(Player player)
    {
        this.player = player;
        playerConnector = player.playerConnector;
        animController = player.animController;
    }

    /// <summary>
    /// The base change player state function decleration which all other states rely on, needs to be in all states to stop errors even if there is no functionality for it in the called state.
    /// </summary>
    /// <param name="playerstate">The state the player needs to transition to</param>
    public virtual void ChangePlayerState(PlayerState playerstate) { }

    /// <summary>
    /// The base Update function decleration which all other states rely on, needs to be in all states to stop errors even if there is no functionality for it in the called state.
    /// </summary>
    public virtual void Update() { }

    /// <summary>
    ///The base movement function decleration which all other states rely on, needs to be in all states to stop errors even if there is no functionality for it in the called state. 
    /// </summary>
    public virtual void Movement() { }

    /// <summary>
    /// The base camera rotation function decleration which all other states rely on, needs to be in all states to stop errors even if there is no functionality for it in the called state.
    /// </summary>
    public virtual void CameraRotationMatching() { }
}

public class PlayerIdleState : PlayerStates
{
    /// <summary>
    /// The states constructor which is defined in the base PlayerStates class
    /// </summary>
    /// <param name="player">The monobehaviour player script that holds the declearation of the state machine</param>
    public PlayerIdleState(Player player) : base(player) { }

    /// <summary>
    /// Trigger by the input system to go into the walking state that provide the continous functionlaity for walking.
    /// </summary>
    public override void Movement()
    {
        ChangePlayerState(PlayerState.Walking);
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
        }
    }

}

public class PlayerWalkingState : PlayerStates
{
    /// <summary>
    /// The states constructor which is defined in the base PlayerStates class
    /// </summary>
    /// <param name="player">The monobehaviour player script that holds the declearation of the state machine</param>
    public PlayerWalkingState(Player player) : base(player) { }

    /// <summary>
    /// A function that represent the Update function of Monobehaviours by having this state function called on the monohaviour player script in the Update function.
    /// Allows for continous functionlaity from the state machine
    /// </summary>
    public override void Update()
    {
        Movement();
        CameraRotationMatching();
    }

    /// <summary>
    /// function that provide the movement for the player by taking the raw input, creating a Vector3 and moves the character controller by that vector3 every second times speed. 
    /// Also handles the animations by changing the X and Y animation controller variables which changes the animations from idle to the correct walking animation via a blend tree.
    /// </summary>
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

    /// <summary>
    /// Function that makes the player rotation match the camera rotation meaning the player will look where the camera is pointing
    /// </summary>
    public override void CameraRotationMatching()
    {
        Quaternion newPlayerRot = player.playerCam.transform.rotation;
        newPlayerRot.x = 0;
        newPlayerRot.z = 0;
        player.transform.rotation = newPlayerRot;
    }

    /// <summary>
    /// Function that changes the player state and performs the functionality to transition into the passed in state
    /// </summary>
    /// <param name="playerstate">The State the player need to transition into</param>
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
