using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class InputManager : MonoBehaviour
{
    #region Dependencies

    [SerializeField] PlayerConnector playerConnector;
    PlayerControls playerControls;

    #endregion

    /// <summary>
    /// Called when the script is enabled including at startup, defines variables and connectors controls.
    /// </summary>
    private void OnEnable()
    {
        //Input Manager Initialisation
        playerControls = new PlayerControls();
        playerControls.Enable();

        //Player Movement Controls
        playerControls.Game.Movement.performed += ctx => playerConnector.currentPlayerState.Movement();
        playerControls.Game.Movement.performed += ctx => playerConnector.movementRaw = ctx.ReadValue<Vector2>();
        playerControls.Game.Movement.canceled += ctx => playerConnector.movementRaw = new Vector2();

        //Player Movement Mode Toggles
        playerControls.Game.WalkToggle.performed += ctx => playerConnector.walkMode = true;
        playerControls.Game.WalkToggle.canceled += ctx => playerConnector.walkMode = false;
        playerControls.Game.SprintToggle.performed += ctx => playerConnector.sprintMode = true;
        playerControls.Game.SprintToggle.canceled += ctx => playerConnector.sprintMode = false;
        playerControls.Game.CrouchToggle.performed += ctx => playerConnector.currentPlayerState.Movement();
        playerControls.Game.CrouchToggle.performed += ctx => playerConnector.crouchMode = true;
        playerControls.Game.CrouchToggle.canceled += ctx => playerConnector.crouchMode = false;

        //Player Jump Control
        playerControls.Game.Jumping.performed += ctx => playerConnector.currentPlayerState.Jump();
    }

    /// <summary>
    /// Called when the script is disabled, removes all functionlaity from the player controls.
    /// </summary>
    private void OnDisable()
    {
        //Input Manager Deactivation
        playerControls.Disable();

        //Player Movement Controls
        playerControls.Game.Movement.performed -= ctx => playerConnector.currentPlayerState.Movement();
        playerControls.Game.Movement.performed -= ctx => playerConnector.movementRaw = ctx.ReadValue<Vector2>();
        playerControls.Game.Movement.canceled -= ctx => playerConnector.movementRaw = new Vector2();

        //Player Movement Mode Toggles
        playerControls.Game.WalkToggle.performed -= ctx => playerConnector.walkMode = true;
        playerControls.Game.WalkToggle.canceled -= ctx => playerConnector.walkMode = false;
        playerControls.Game.SprintToggle.performed -= ctx => playerConnector.sprintMode = true;
        playerControls.Game.SprintToggle.canceled -= ctx => playerConnector.sprintMode = false;
        playerControls.Game.CrouchToggle.performed -= ctx => playerConnector.crouchMode = true;
        playerControls.Game.CrouchToggle.canceled -= ctx => playerConnector.crouchMode = false;
    }

}
