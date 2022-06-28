using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunningAttackState : PlayerStates
{
    public PlayerRunningAttackState(Player player) : base(player) { }

    public override void Init()
    {
        playerConnector.combatMode = true;
        playerConnector.crouchMode = false;
        playerConnector.sprintMode = false;
        playerConnector.walkMode = false;

        animController.SetFloat(playerConnector.animMovementXHash, 0);
        animController.SetFloat(playerConnector.animMovementYHash, 0);

        if(playerConnector.leftHandRunningAttack)
        {
            animController.SetTrigger(playerConnector.animLeftHandRunningAttack);
            playerConnector.combatMode = true;
            animController.SetBool(playerConnector.animCombatBool, playerConnector.combatMode);
        }


        if (playerConnector.rightHandRunningAttack)
        {
            animController.SetTrigger(playerConnector.animRightHandRunningAttack);
            playerConnector.combatMode = true;
            animController.SetBool(playerConnector.animCombatBool, playerConnector.combatMode);
        }
    }

    public override void Update()
    {
        base.Update();
        CameraRotationMatching();
    }

    public override void ChangePlayerState(PlayerState playerstate)
    {
        switch(playerstate)
        {
            case PlayerState.Idle:
                playerConnector.playerState = PlayerState.Idle;
                playerConnector.currentPlayerState = new PlayerIdleState(player);
                Debug.Log("Changed Player State to Idle");
                break;
        }
        playerConnector.currentPlayerState.Init();
    }

}
