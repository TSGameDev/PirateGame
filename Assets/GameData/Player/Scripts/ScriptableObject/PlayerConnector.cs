using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Connector", menuName = "ScritpableObjects/Connector/Player")]
public class PlayerConnector : ScriptableObject
{
    #region Movement Variables

    public Vector2 movementRaw;
    public float speed = 10f;

    #endregion

    #region AnimHashes

    public readonly int animMovementXHash = Animator.StringToHash("MovementX");
    public readonly int animMovementYHash = Animator.StringToHash("MovementY");

    #endregion

    #region State Machine

    public PlayerState playerState;
    public PlayerStates currentPlayerState;

    #endregion

    private void Awake()
    {
        playerState = PlayerState.Idle;
        
    }
}
