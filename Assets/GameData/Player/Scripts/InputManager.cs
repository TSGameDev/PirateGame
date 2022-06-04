using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class InputManager : MonoBehaviour
{
    #region Dependencies

    [SerializeField] PlayerConnector playerConnector;
    PlayerControls playerControls;
    Player player;

    #endregion

    private void OnEnable()
    {
        player = GetComponent<Player>();
        playerControls = new PlayerControls();
        playerControls.Enable();

        playerControls.Game.Movement.performed += ctx => playerConnector.movementRaw = ctx.ReadValue<Vector2>();
        playerControls.Game.Movement.canceled += ctx => playerConnector.movementRaw = new Vector2();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
}
