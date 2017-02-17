﻿using UnityEngine;

public class RobotAttack1State : RobotState {
    public override State HandleInput(StateMachine stateMachine,
        XboxInput xboxInput) {
        if (!(stateMachine is RobotStateMachine)) return null;

        RobotStateMachine robotStateMachine = (RobotStateMachine)stateMachine;

        if (!this.IsAnimationPlaying(robotStateMachine, "RobotAttack1")) {
            return null;
        }

        if (Input.GetKeyDown(xboxInput.A)) {
            return new RobotAttack2State();
        }

        if (this.IsCurrentAnimationFinished(robotStateMachine)) {
            if (this.IsLastState(robotStateMachine, "RobotWalkState")) {
                return new RobotWalkState();
            }

            return new RobotIdleState();
        }

        return null;
    }

    public override void Update(StateMachine stateMachine) {
        if (!(stateMachine is RobotStateMachine)) return;

        ((RobotStateMachine)stateMachine).PlayerController.Movement();
    }

    public override void Enter(StateMachine stateMachine) {
        Debug.Log("ATTACK1 ENTER!");
    }

    public override void Exit(StateMachine stateMachine) {
        Debug.Log("ATTACK1 EXIT!");
    }
}
