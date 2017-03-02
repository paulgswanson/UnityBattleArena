﻿using UnityEngine;

public class DebugViewPlayerAttacks : DebugViewCurrentState {
    public Color ColorActive = Color.red;

    protected Color DefaultColor = Color.green;
    protected RobotAttackState RobotAttackState = null;

    void Start () {
        this.TryToGetStateMachine();

        if (this.StateMachine == null) return;

        this.ColorMeshes(this.DefaultColor);
    }

    void Update () {
        this.TryToGetAttack();

        if (this.RobotAttackState == null) return;

        if (HandleHit.IsAttackActive(this.RobotAttackState)) {
            this.ColorMeshes(this.ColorActive);
        } else {
            this.ColorMeshes(this.DefaultColor);
        }
    }

    protected virtual void TryToGetAttack() {
        this.TryToGetStateMachine();

        if (this.StateMachine == null) return;

        if (!(this.StateMachine.CurrentState is RobotAttackState)) return;

        this.RobotAttackState = 
            (RobotAttackState) this.StateMachine.CurrentState;
    }

    protected virtual void ColorMeshes(Color color) {
        MeshRenderer[] meshRenderers =
            this.StateMachine.GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer meshRenderer in meshRenderers) {
            meshRenderer.material.color = color;
        }
    }
}
