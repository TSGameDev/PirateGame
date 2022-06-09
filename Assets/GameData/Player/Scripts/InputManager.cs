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

    private void OnEnable()
    {
        //Input Manager Initialisation
        playerControls = new PlayerControls();
        playerControls.Enable();

        //Player Movement Controls
        playerControls.Game.Movement.performed += ctx => playerConnector.currentPlayerState.Movement();
        playerControls.Game.Movement.performed += ctx => playerConnector.movementRaw = ctx.ReadValue<Vector2>();
        playerControls.Game.Movement.canceled += ctx => playerConnector.movementRaw = new Vector2();
        playerControls.Game.WalkToggle.performed += ctx => playerConnector.walkMode = true;
        playerControls.Game.WalkToggle.canceled += ctx => playerConnector.walkMode = false;
        playerControls.Game.SprintToggle.performed += ctx => playerConnector.sprintMode = true;
        playerControls.Game.SprintToggle.canceled += ctx => playerConnector.sprintMode = false;
    }

    private void OnDisable()
    {
        //Input Manager Deactivation
        playerControls.Disable();

        //Player Movement Controls
        playerControls.Game.Movement.performed -= ctx => playerConnector.currentPlayerState.Movement();
        playerControls.Game.Movement.performed -= ctx => playerConnector.movementRaw = ctx.ReadValue<Vector2>();
        playerControls.Game.Movement.canceled -= ctx => playerConnector.movementRaw = new Vector2();
        playerControls.Game.WalkToggle.performed -= ctx => playerConnector.walkMode = true;
        playerControls.Game.WalkToggle.canceled -= ctx => playerConnector.walkMode = false;
        playerControls.Game.SprintToggle.performed -= ctx => playerConnector.sprintMode = true;
        playerControls.Game.SprintToggle.canceled -= ctx => playerConnector.sprintMode = false;
    }

}
