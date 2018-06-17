using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerMovement : NetworkBehaviour {


    private bool MovePanelBool;
    private float speed = 10f;
    private PlayerV2 player;


    private void Start(){
        player = GetComponent<PlayerV2>();
    }

    private void Update(){
        if (isLocalPlayer){
            if (MovePanelBool && GameManagerV2.IsTurn(player.playerId)){
                CmdMovePanel();
            }
        }

    }

    public void MoveButtonDown(){
        MovePanelBool = true;
    }

    public void MoveButtonUp(){
        MovePanelBool = false;
    }


    [Command]
    private void CmdMovePanel(){
        GameManagerV2.MovePanel(speed);
        //Vector2 temp = GameManagerV2.single.panel.transform.position;
        //temp.x += speed * Time.deltaTime;
        //GameManagerV2.single.panel.transform.position = temp;

    }

}

/*
     if (isLocalPlayer){
            player = GetComponent<Player>();
            //AddButtonFunctions();
            playerGUI.gameObject.SetActive(true);

        }

    }

    private void Update () {
        if (!isLocalPlayer){
            return;
        }

        if (player.myTurn && movePlayer){
            Debug.Log("moveee");
            MovePlayer();
        }

        playerGUI.turn.text = "turn " + player.myTurn.ToString();

    }

    private void MovePlayer(){
        Vector2 temp = GameManager.instance.panel.transform.position; 
        temp.x += speed * Time.deltaTime;
        GameManager.instance.panel.transform.position = temp;
       
    } 

    public void MoveButtonDown(){
        movePlayer = true;
    }

    public void MoveButtonUp(){
        movePlayer = false;
    }

    [Command]
    public void CmdEndTurn(){
        if (player.myTurn){
            GameManager.NextTurn();
        }

        //GameManager.single.leftMovementPosible = !GameManager.single.leftMovementPosible; 
        //player.myTurn = false;
    }


    public void AddButtonFunctions(){
        if (!isLocalPlayer){
            return;
        }

        playerGUI.left.text = "left " + player.playerControlsLeftDirection.ToString();

        if (player.playerControlsLeftDirection){
            Debug.Log("addplayerControlsLeftDirection");

            EventTrigger triggerDown = playerGUI.leftSideBut.GetComponentInParent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry{ eventID = EventTriggerType.PointerDown };
            entry.callback.AddListener((eventData) => { MoveButtonDown(); });
            triggerDown.triggers.Add(entry);

            EventTrigger triggerUp = playerGUI.leftSideBut.GetComponentInParent<EventTrigger>();
            EventTrigger.Entry entry1 = new EventTrigger.Entry{ eventID = EventTriggerType.PointerUp };
            entry1.callback.AddListener((eventData) => { MoveButtonUp(); });
            triggerUp.triggers.Add(entry1);

            playerGUI.rightSideBut.GetComponent<Button>().onClick.AddListener(CmdEndTurn);

            ColorBlock cb = playerGUI.rightSideBut.colors;
            cb.normalColor = Color.green;
            playerGUI.rightSideBut.colors = cb;

            ColorBlock cb1 = playerGUI.leftSideBut.colors;
            cb1.normalColor = Color.red;
            playerGUI.leftSideBut.colors = cb1;

            speed = -speed;
        }
        else{
            Debug.Log("add");

            EventTrigger triggerDown = playerGUI.rightSideBut.GetComponentInParent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry{ eventID = EventTriggerType.PointerDown };
            entry.callback.AddListener((eventData) => { MoveButtonDown(); });
            triggerDown.triggers.Add(entry);

            EventTrigger triggerUp = playerGUI.rightSideBut.GetComponentInParent<EventTrigger>();
            EventTrigger.Entry entry1 = new EventTrigger.Entry{ eventID = EventTriggerType.PointerUp };
            entry1.callback.AddListener((eventData) => { MoveButtonUp(); });
            triggerUp.triggers.Add(entry1);

            playerGUI.leftSideBut.GetComponent<Button>().onClick.AddListener(CmdEndTurn);

            ColorBlock cb = playerGUI.rightSideBut.colors;
            cb.normalColor = Color.red;
            playerGUI.rightSideBut.colors = cb;

            ColorBlock cb1 = playerGUI.leftSideBut.colors;
            cb1.normalColor = Color.green;
            playerGUI.leftSideBut.colors = cb1;
        }
    } 
    */
