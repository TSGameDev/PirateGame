using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New Player Connector", menuName = "ScritpableObjects/Connector/Player")]
public class PlayerConnector : ScriptableObject
{
    #region Player Stats

    [TabGroup("base", "Player Stats")]

    [FoldoutGroup("base/Player Stats/Stamina")]
    [MinValue("@staminaRange.x"), MaxValue("@staminaRange.y")]
    [PropertyTooltip("The current stamina of the Player.")]
    public float stamina = 100;

    [FoldoutGroup("base/Player Stats/Stamina")]
    [MinMaxSlider(0,300, true)]
    [PropertyTooltip("The range the player stamina is locked to. Left number is the min stamina, Right number is max stamina.")]
    public Vector2 staminaRange = new Vector2(0, 200);

    [FoldoutGroup("base/Player Stats/Stamina")]
    [MinValue(1), MaxValue(10)]
    [PropertyTooltip("The stamina regeneration rate per second")]
    public float staminaRegenRate = 1;

    [FoldoutGroup("base/Player Stats/Stamina")]
    [MinValue(1), MaxValue(50)]
    [PropertyTooltip("The stamina required to begin running. Only detemines the amount of stamina to begin running does not actually reduce stamina, Used to prevent weird glitching when stamina regens at 0 and then runs again.")]
    public float sprintingStaminaStartCost = 10;

    [FoldoutGroup("base/Player Stats/Stamina")]
    [MinValue(0), MaxValue(10)]
    [PropertyTooltip("The stamina cost per second when running.")]
    public float sprintingCost = 1;

    #endregion

    #region Movement Flags

    [TabGroup("base", "Movement")]

    [FoldoutGroup("base/Movement/Movement Flags")]
    [HorizontalGroup("base/Movement/Movement Flags/Hoz1", LabelWidth = 75)]
    [PropertyTooltip("Toggle to enter Walking state.")]
    public bool walkMode = false;

    [HorizontalGroup("base/Movement/Movement Flags/Hoz1", LabelWidth = 75)]
    [PropertyTooltip("Toggle to enter Sprinting state.")]
    public bool sprintMode = false;

    [HorizontalGroup("base/Movement/Movement Flags/Hoz1", LabelWidth = 75)]
    [PropertyTooltip("Toggle to enter Crouching state.")]
    public bool crouchMode = false;

    [HorizontalGroup("base/Movement/Movement Flags/Hoz1", LabelWidth = 75)]
    [PropertyTooltip("Toggle to enter Combat Mode.")]
    public bool combatMode = false;

    #endregion

    #region Movement Variables

    [HideInInspector]
    //Variable that stores the input used for movement that is not normalised.
    public Vector2 movementRaw;
    
    [FoldoutGroup("base/Movement/Movement Speeds")]
    [PropertyTooltip("The player walking speed.")]
    public float walkSpeed = 2f;
    
    [FoldoutGroup("base/Movement/Movement Speeds")]
    [PropertyTooltip("The player running speed.")]
    public float runSpeed = 5f;
    
    [FoldoutGroup("base/Movement/Movement Speeds")]
    [PropertyTooltip("The player sprint speed.")]
    public float sprintSpeed = 10f;

    #endregion

    #region Gravity Variables

    [FoldoutGroup("base/Movement/Gravity")]
    [PropertyTooltip("The change per second to gravity applied to player when not grounded.")]
    public float gravity;
    [FoldoutGroup("base/Movement/Gravity")]
    [PropertyTooltip("The constant gravity to be applied to the player. Keeps the player on the ground while moving.")]
    public float constantGravity;
    [FoldoutGroup("base/Movement/Gravity")]
    [PropertyTooltip("The max gravity that can be enforced on the player.")]
    public float maxGravity;

    #endregion

    #region Jumping / Falling

    [HideInInspector]
    //The current Y velocity of the player
    public float fallingSpeed;
    [FoldoutGroup("base/Movement/Jumping & Falling")]
    [PropertyTooltip("The threshold that controls falling animations. The falling speed needs to be greater than this for falling animations to play.")]
    public float fallingThresHold;
    [FoldoutGroup("base/Movement/Jumping & Falling")]
    [PropertyTooltip("The force applied to the player when jumping. Determines jump height.")]
    public float jumpForce;

    [HideInInspector]
    //Bool to show if the player has jumped/ is jumpping or falling
    public bool jumpingTriggered = false;
    [HideInInspector]
    //Bool to show if the player is currently falling
    public bool fallingTriggered = false;

    #endregion

    #region Combat
    
    [TabGroup("base", "Combat")]
    [FoldoutGroup("base/Combat/Combo")]
    [PropertyTooltip("Bool for timing transitions to combo attacks")]
    public bool comboPossible = false;

    [FoldoutGroup("base/Combat/Combo")]
    [PropertyTooltip("The current step/attack number in a combo, Used to track if comboPossible is required to trigger anim.")]
    public int comboStep = 0;

    [FoldoutGroup("base/Combat/Combat Flags")]
    [PropertyTooltip("A bool flag to know if a running attack is right handed.")]
    public bool rightHandRunningAttack = false;

    [FoldoutGroup("base/Combat/Combat Flags")]
    [PropertyTooltip("A bool flag to know if a running attack is left handed")]
    public bool leftHandRunningAttack = false;

    #endregion

    #region State Machine

    [TabGroup("base", "Player Stats"), PropertyOrder(-1)]
    [PropertyTooltip("The current state the player is in.")]
    public PlayerState playerState;
    public PlayerStates currentPlayerState;

    #endregion

    #region AnimHashes

    public readonly int animWalkBool = Animator.StringToHash("WalkToggle");
    public readonly int animSprintBool = Animator.StringToHash("SprintToggle");
    public readonly int animCrouchBool = Animator.StringToHash("CrouchToggle");
    public readonly int animCombatBool = Animator.StringToHash("CombatToggle");
    public readonly int animMovementXHash = Animator.StringToHash("MovementX");
    public readonly int animMovementYHash = Animator.StringToHash("MovementY");
    public readonly int animJumpHash = Animator.StringToHash("Jump");
    public readonly int animFallingHash = Animator.StringToHash("Falling");
    public readonly int animLandHash = Animator.StringToHash("Land");
    public readonly int animLandRollRun = Animator.StringToHash("LandRollRun");
    public readonly int animDuelWieldAttack = Animator.StringToHash("DualWieldAttack");
    public readonly int animDuelWieldAttackCombo = Animator.StringToHash("DualWieldAttackCombo");
    public readonly int animParry = Animator.StringToHash("Parry");
    public readonly int animParryHit = Animator.StringToHash("ParryHit");
    public readonly int animLeftHandAttack = Animator.StringToHash("LeftHandAttack");
    public readonly int animLeftHandAttackCombo = Animator.StringToHash("LeftHandAttackCombo");
    public readonly int animLeftHandRunningAttack = Animator.StringToHash("LeftHandRunningAttack");
    public readonly int animShieldAttack = Animator.StringToHash("ShieldAttack");
    public readonly int animShieldAttackCombo = Animator.StringToHash("ShieldAttackCombo");
    public readonly int animRightHandAttack = Animator.StringToHash("RightHandAttack");
    public readonly int animRightHandAttackCombo = Animator.StringToHash("RightHandAttackCombo");
    public readonly int animRightHandRunningAttack = Animator.StringToHash("RightHandRunningAttack");
    
    #endregion

    #region Events

    public event UnityAction OnStaminaChanged;

    #endregion

    private void Awake()
    {
        playerState = PlayerState.Idle;
    }

}
