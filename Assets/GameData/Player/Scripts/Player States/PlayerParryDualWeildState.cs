using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParryDualWeildState : PlayerStates
{
    public PlayerParryDualWeildState(Player player) : base(player) { }

    public override void Init()
    {
        animController.SetFloat(playerConnector.animMovementXHash, 0);
        animController.SetFloat(playerConnector.animMovementXHash, 0);
        ParryDualAttack();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void ParryDualAttack()
    {
        if(playerConnector.dualWeildEquiped)
        {
            switch (playerConnector.comboStep)
            {
                case 0:
                    animController.SetTrigger(playerConnector.animDuelWieldAttack);
                    playerConnector.comboStep++;
                    playerConnector.comboPossible = false;
                    playerConnector.combatMode = true;
                    animController.SetBool(playerConnector.animCombatBool, playerConnector.combatMode);
                    return;
                case 1:
                    if (playerConnector.comboPossible)
                    {
                        animController.SetTrigger(playerConnector.animDuelWieldAttackCombo);
                        playerConnector.comboStep++;
                        playerConnector.comboPossible = false;
                        playerConnector.combatMode = true;
                        animController.SetBool(playerConnector.animCombatBool, playerConnector.combatMode);
                    }
                    return;
            }
        }

        else if(playerConnector.shieldEquiped)
        {
            animController.SetTrigger(playerConnector.animShieldParry);
            playerConnector.combatMode = true;
        }

        else
        {
            animController.SetTrigger(playerConnector.animParry);
            playerConnector.combatMode = true;
        }
    }

    public override void CameraRotationMatching()
    {
        base.CameraRotationMatching();
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
