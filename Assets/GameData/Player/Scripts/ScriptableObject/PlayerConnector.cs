using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Player Connector", menuName = "ScritpableObjects/Connector/Player")]
public class PlayerConnector : ScriptableObject
{
    #region Player Stats

    public float stamina = 100;

    [Range(50, 200)]
    public float maxStamina = 100;
    public float sprintingStaminaStartCost = 10;

    #endregion

    #region Movement Flags

    public bool walkMode = false;
    public bool sprintMode = false;
    public bool crouchMode = false;

    #endregion

    #region Movement Variables

    public Vector2 movementRaw;
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float sprintSpeed = 10f;

    #endregion

    #region Gravity Variables

    public float gravity;
    public float constantGravity;
    public float maxGravity;

    #endregion

    #region Jumping / Falling

    public float fallingSpeed;
    public float fallingThresHold;
    public float jumpForce;

    public bool jumpingTriggered = false;
    public bool fallingTriggered = false;

    #endregion

    #region AnimHashes

    public readonly int animWalkBool = Animator.StringToHash("WalkToggle");
    public readonly int animSprintBool = Animator.StringToHash("SprintToggle");
    public readonly int animCrouchBool = Animator.StringToHash("CrouchToggle");
    public readonly int animMovementXHash = Animator.StringToHash("MovementX");
    public readonly int animMovementYHash = Animator.StringToHash("MovementY");
    public readonly int animJumpHash = Animator.StringToHash("Jump");
    public readonly int animFallingHash = Animator.StringToHash("Falling");
    public readonly int animLandHash = Animator.StringToHash("Land");
    public readonly int animLandRollRun = Animator.StringToHash("LandRollRun");

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
