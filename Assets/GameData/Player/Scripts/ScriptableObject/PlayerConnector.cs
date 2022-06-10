using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Player Connector", menuName = "ScritpableObjects/Connector/Player")]
public class PlayerConnector : ScriptableObject
{
    #region Player Stats

    public float stamina
    {
        get
        {
            return stamina;
        }

        set
        {
            if (value > maxStamina)
                stamina = maxStamina;
            else
                stamina = value;
        }
    }

    [Range(50, 200)]
    public float maxStamina = 100;
    public float sprintingStaminaCost = 10;

    #endregion

    #region Movement Variables

    public bool walkMode;
    public bool sprintMode;
    public bool crouchMode;
    public Vector2 movementRaw;
    public float speed = 10f;

    #endregion

    #region AnimHashes
    
    public readonly int animWalkBool = Animator.StringToHash("WalkToggle");
    public readonly int animSprintBool = Animator.StringToHash("SprintToggle");
    public readonly int animCrouchBool = Animator.StringToHash("CrouchToggle");
    public readonly int animMovementXHash = Animator.StringToHash("MovementX");
    public readonly int animMovementYHash = Animator.StringToHash("MovementY");

    #endregion

    #region State Machine

    public PlayerState playerState;
    public PlayerStates currentPlayerState;

    #endregion

    #region Events

    public event UnityAction OnStaminaChanged;

    #endregion

    private void Awake()
    {
        playerState = PlayerState.Idle;
    }

}
