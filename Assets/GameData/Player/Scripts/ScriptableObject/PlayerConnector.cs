using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New Player Connector", menuName = "ScritpableObjects/Connector/Player")]
public class PlayerConnector : ScriptableObject
{
    #region Player Stats

    [TabGroup("base", "Player Stats")]

    [FoldoutGroup("base/Player Stats/Stamina")]
    [MinValue("@maxStamina.x"), MaxValue("@maxStamina.y")]
    public float stamina = 100;

    [FoldoutGroup("base/Player Stats/Stamina")]
    [MinMaxSlider(0,300, true)]
    public Vector2 maxStamina = new Vector2(0, 200);

    [FoldoutGroup("base/Player Stats/Stamina")]
    [MinValue(1), MaxValue(50)]
    public float sprintingStaminaStartCost = 10;

    #endregion

    #region Movement Flags

    [TabGroup("base", "Movement")]

    [FoldoutGroup("base/Movement/Movement Flags")]
    [HorizontalGroup("base/Movement/Movement Flags/Hoz1", LabelWidth = 100)]
    public bool walkMode = false;

    [HorizontalGroup("base/Movement/Movement Flags/Hoz1", LabelWidth = 100)]
    public bool sprintMode = false;

    [HorizontalGroup("base/Movement/Movement Flags/Hoz1", LabelWidth = 100)]
    public bool crouchMode = false;

    #endregion

    #region Movement Variables
    
    [FoldoutGroup("base/Movement/Movement Speeds")]
    public Vector2 movementRaw;
    
    [FoldoutGroup("base/Movement/Movement Speeds")]
    public float walkSpeed = 2f;
    
    [FoldoutGroup("base/Movement/Movement Speeds")]
    public float runSpeed = 5f;
    
    [FoldoutGroup("base/Movement/Movement Speeds")]
    public float sprintSpeed = 10f;

    #endregion

    #region Gravity Variables

    [FoldoutGroup("base/Movement/Gravity")]
    public float gravity;
    [FoldoutGroup("base/Movement/Gravity")]
    public float constantGravity;
    [FoldoutGroup("base/Movement/Gravity")]
    public float maxGravity;

    #endregion

    #region Jumping / Falling

    [FoldoutGroup("base/Movement/Jumping & Falling")]
    public float fallingSpeed;
    [FoldoutGroup("base/Movement/Jumping & Falling")]
    public float fallingThresHold;
    [FoldoutGroup("base/Movement/Jumping & Falling")]
    public float jumpForce;

    [FoldoutGroup("base/Movement/Jumping & Falling")]
    public bool jumpingTriggered = false;
    [FoldoutGroup("base/Movement/Jumping & Falling")]
    public bool fallingTriggered = false;

    #endregion

    #region State Machine

    [TabGroup("base", "Player Stats"), PropertyOrder(-1)]
    public PlayerState playerState;
    public PlayerStates currentPlayerState;

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

    #region Events

    public event UnityAction OnStaminaChanged;

    #endregion

    private void Awake()
    {
        playerState = PlayerState.Idle;
    }

}
