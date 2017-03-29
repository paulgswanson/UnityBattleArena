﻿using System;
using UnityEngine;

public class HandleHit : Photon.MonoBehaviour {
    protected PlayerController PlayerController = null;

    void Start() {
        this.PlayerController 
            = this.transform.root.GetComponent<PlayerController>();
    }

    void OnCollisionEnter(Collision other) {
        if (!this.CheckIfValid()) return;

        this.HandleOpponent(other);
        this.HandlePlayer(other);
    }

    protected virtual bool CheckIfValid() {
        return this.photonView.isMine && 
            this.transform.root.CompareTag(PlayerController.Player);
    }

    protected virtual void HandleOpponent(Collision other) {
        if (!other.transform.root.CompareTag(PlayerController.Opponent)) {
            return;
        }

        if (!(this.PlayerController.RobotStateMachine.CurrentState is 
            RobotAttackState)) {
            return;
        }

        RobotAttackState robotAttackState = 
            (RobotAttackState)this.PlayerController.RobotStateMachine
            .CurrentState;

        robotAttackState.HandleAttack(this, other);
    }

    void OnTriggerEnter(Collider other) {
        Debug.Log("OTS " + DateTime.Now.ToShortTimeString());
        if (!this.CheckIfValid()) return;

        this.HandleOpponentTrigger(other);
        this.HandlePlayerTrigger(other);
    }

    protected virtual void HandlePlayerTrigger(Collider other) {
        if (!other.transform.root.CompareTag(PlayerController.Player)) return;
    }

    protected virtual void HandleOpponentTrigger(Collider other) {
        Debug.Log("HANDLETRIGGER1 " + DateTime.Now.ToShortTimeString());

        if (!other.transform.root.CompareTag(PlayerController.Opponent)) {
            Debug.Log(other.gameObject.tag);
            return;
        }

        Debug.Log("HANDLETRIGGER2 " + DateTime.Now.ToShortTimeString());

        if (!(this.PlayerController.RobotStateMachine.CurrentState is
            RobotAttackState)) {
            return;
        }

        Debug.Log("HANDLETRIGGER3 " + DateTime.Now.ToShortTimeString());

        RobotAttackState robotAttackState =
            (RobotAttackState)this.PlayerController.RobotStateMachine
            .CurrentState;

        Debug.Log("HANDLETRIGGER4 " + DateTime.Now.ToShortTimeString());

        robotAttackState.HandleAttackTrigger(this, other);
    }

    public virtual void SendHit(GameObject other, int damage, int hitstun) {
        int opponentID = -1;

        if ((opponentID = this.GetOpponentID(other)) == -1) {
            return;
        }

        this.photonView.RPC("ReceiveHit", PhotonTargets.AllViaServer, damage, 
            hitstun, opponentID);
    }

    public virtual void SendPoke(GameObject other, Vector3 direction) {
        int opponentID = -1;

        if ((opponentID = this.GetOpponentID(other)) == -1) {
            return;
        }

        Debug.Log("SENDPOKE " + opponentID + " " + DateTime.Now.ToShortTimeString());

        this.photonView.RPC("ReceivePoke", PhotonTargets.Others, 
            direction, opponentID);
    }

    protected virtual void HandlePlayer(Collision other) {
        if (!other.transform.root.CompareTag(PlayerController.Player)) return;
    }

    protected virtual void SendHitstun(PlayerController who, int hitstun) {
		if(who!=null)
        who.RobotStateMachine.SetState(new RobotHitstunState(hitstun));
    }

    protected virtual int GetOpponentID(GameObject other) {
        PlayerController opponentController = 
            other.transform.root.GetComponent<PlayerController>();

        if (opponentController == null) {
            return -1;
        }

        return opponentController.ID;
    }

    [PunRPC]
    public void ReceiveHit(int damage, int hitstun, int playerID) {
        /* Used once per client, so we need to send the hit to the right 
         * Robot! */
        PlayerController who = 
            GameManager.Instance.PlayerList[playerID].PlayerController;

        who.PlayerHealth.Health -= damage;
        this.SendHitstun(who, hitstun);
    }

    [PunRPC]
    public void ReceivePoke(Vector3 direction, int playerID) {
        /* Used once per client, so we need to send the hit to the right 
         * Robot! */

        PlayerController who =
            GameManager.Instance.PlayerList[playerID].PlayerController;

        Debug.Log("RECEIVEDPOKE " + playerID + " " + DateTime.Now.ToShortTimeString());

        who.PlayerPhysics.ApplyPoke(direction);
    }
}