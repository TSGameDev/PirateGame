using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerStates
{
    public PlayerAttackState(Player player) : base(player) { }

    public override void Init()
    {
        playerConnector.walkMode = true;
        playerConnector.combatMode = true;
        playerConnector.crouchMode = false;
        playerConnector.sprintMode = false;
        animController.SetBool(playerConnector.animWalkBool, true);

        if (playerConnector.leftHandAttack)
            LeftHandAttack();

        if(playerConnector.rightHandAttack)
            RightHandAttack();
    }

    public override void Update()
    {
        base.Update();
        Movement();
        CameraRotationMatching();
    }

    public override void Movement()
    {
        float rawX = playerConnector.movementRaw.x;
        float rawY = playerConnector.movementRaw.y;

        animController.SetFloat(playerConnector.animMovementXHash, rawX);
        animController.SetFloat(playerConnector.animMovementYHash, rawY);

        movement = (player.transform.right * rawX) + (player.transform.forward * rawY);
        movement = movement.normalized * playerConnector.walkSpeed * Time.deltaTime;

        if (movement.magnitude >= Mathf.Epsilon)
            player.characterController.Move(movement + gravityMovement);
    }

    public override void LeftHandAttack()
    {
        switch (playerConnector.comboStep)
        {
            case 0:
                animController.SetTrigger(playerConnector.animLeftHandAttack);
                playerConnector.comboStep++;
                playerConnector.comboPossible = false;
                playerConnector.combatMode = true;
                animController.SetBool(playerConnector.animCombatBool, playerConnector.combatMode);
                return;
            case 1:
                if (playerConnector.comboPossible)
                {
                    animController.SetTrigger(playerConnector.animLeftHandAttackCombo);
                    playerConnector.comboStep++;
                    playerConnector.comboPossible = false;
                    playerConnector.combatMode = true;
                    animController.SetBool(playerConnector.animCombatBool, playerConnector.combatMode);
                }
                return;
        }
    }

    public override void RightHandAttack()
    {
        switch (playerConnector.comboStep)
        {
            case 0:
                animController.SetTrigger(playerConnector.animRightHandAttack);
                playerConnector.comboStep++;
                playerConnector.comboPossible = false;
                playerConnector.combatMode = true;
                animController.SetBool(playerConnector.animCombatBool, playerConnector.combatMode);
                return;
            case 1:
            case 2:
                if (playerConnector.comboPossible)
                {
                    animController.SetTrigger(playerConnector.animRightHandAttackCombo);
                    playerConnector.comboStep++;
                    playerConnector.comboPossible = false;
                    playerConnector.combatMode = true;
                    animController.SetBool(playerConnector.animCombatBool, playerConnector.combatMode);
                }
                return;
        }
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
