using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

    [HideInInspector]
    public bool ready = false;
    public bool playerControlsLeftDirection;

    [SyncVar]
    public bool myTurn;

    [SyncVar]
    public int playerId;

    private PlayerMovement playerMove;

 

}



/*

       private void Awake(){
        if (isLocalPlayer){
            playerMove = GetComponent<PlayerMovement>();
        }
    }

    private void Start() {
        if (isServer){
            playerId = GameManager.Register(this);
            if (playerId == 1){
                playerControlsLeftDirection = true;
            }
        }

        if (!isLocalPlayer){
            return;
        }

        CmdSetReady();
        //ButInit();
        ready = true;
    }

    private void Update(){
        if (!isLocalPlayer){
            return;
        }

        if (GameManager.IsTurn(playerId)){
            myTurn = true;
        }
        else{
            myTurn = false;
        }
    }

    [Command]
    private void CmdSetReady(){
        ready = true;
    }

    [TargetRpc]
    public void TargetButInit(NetworkConnection target) {
        //if (isLocalPlayer){
            ButInit();
            Debug.Log("RpcButInit");
        //}
    }

    public void ButInit(){
        playerMove.AddButtonFunctions();
    }
    */
