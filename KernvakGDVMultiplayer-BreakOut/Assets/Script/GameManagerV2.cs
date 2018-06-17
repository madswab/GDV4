using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameManagerV2 : NetworkBehaviour {

    public static GameManagerV2 single;

    public int maxPlayers = 2; //hard
    public PlayerV2 currentTurnPlayer;
    public int currentPlayerIndex = 0;
    public Text livesText;
    public Text scoreText;
    public List<PlayerV2> players = new List<PlayerV2>();

    public GameObject panel;
    public List<Ball> balls = new List<Ball>();

    private int playerId = 0;

    private void Awake(){
        single = this;

    }

    public static int AddPlayer(PlayerV2 p){
        p.playerId = single.playerId;
        single.players.Add(p);
        Debug.Log("Registered player " + single.playerId);

        if (p.playerId == 0){
            p.playerControlsLeftDirection = true;
        }

        if (GameManagerV2.single.players.Count == GameManagerV2.single.maxPlayers){
            GameObject b = Instantiate(Resources.Load("Ball")) as GameObject;
            NetworkServer.Spawn(b);
            GameManagerV2.single.balls.Add(b.GetComponent<Ball>());
        }

        return single.playerId++;
    }


    /*
    [Client]
    public static void ClientDisableAllButtons(PlayerV2 p){
        //p.ui.leftSideBut.interactable = false;
        //p.ui.rightSideBut.interactable = false;
        //p.ui.leftSideBut.gameObject.SetActive(false);
        //p.ui.rightSideBut.gameObject.SetActive(false);

    }

    [Client]
    public static void EnablePlayHandButtons(PlayerV2 p){
        //buttonStay.interactable = true;
        //p.ui.leftSideBut.interactable = true;
        //p.ui.rightSideBut.interactable = true;
        //p.ui.leftSideBut.gameObject.SetActive(true);
        //p.ui.rightSideBut.gameObject.SetActive(true);
    }*/


    public static bool IsTurn(int id){
        if (single.currentPlayerIndex > single.players.Count){
            single.currentPlayerIndex = 0;
            Debug.Log(single.currentPlayerIndex + " " + single.players.Count);
            Debug.Log("Fked UP");
        }

        return single.currentPlayerIndex == id;
    }

    //[Server]
    public static void NextPlayer(){
        if (single.currentTurnPlayer != null){
            single.currentTurnPlayer.RpcYourTurn(false);
        }

        single.currentPlayerIndex += 1;

        if (single.currentPlayerIndex >= single.players.Count){
            single.currentPlayerIndex = 0;
        }

        single.currentTurnPlayer = single.players[single.currentPlayerIndex];
        single.currentTurnPlayer.RpcYourTurn(true);

        #region
        /*
                while (single.currentPlayerIndex <= single.players.Count){
                    single.currentTurnPlayer = single.players[single.currentPlayerIndex - 1];

                    if (single.currentTurnPlayer != null){
                        Debug.Log("Player " + single.currentPlayerIndex + " turn." );
                        single.currentTurnPlayer.RpcYourTurn(true);
                        break;
                    }

                    single.currentPlayerIndex += 1;
                }

                if (single.currentPlayerIndex >= single.players.Count){
                    single.currentPlayerIndex = 0;
                    single.currentTurnPlayer = single.players[0];

                    Debug.Log("new Round, player " + single.currentPlayerIndex + " turn.");
                }*/
        #endregion
    }

    public static void MovePanel(float speed) {
        Vector2 temp = single.panel.transform.position;
        temp.x += speed * Time.deltaTime;
        single.panel.transform.position = temp;
    }


    /*
    [Client]
    public static void AddButtonFunctions(PlayerV2 p){
        if (p.playerControlsLeftDirection){
            Debug.Log("addplayerControlsLeftDirection");

            EventTrigger triggerDown = p.ui.leftSideBut.GetComponentInParent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry{ eventID = EventTriggerType.PointerDown };
            entry.callback.AddListener((eventData) => { p.pMove.MoveButtonDown(); });
            triggerDown.triggers.Add(entry);

            EventTrigger triggerUp = p.ui.leftSideBut.GetComponentInParent<EventTrigger>();
            EventTrigger.Entry entry1 = new EventTrigger.Entry{ eventID = EventTriggerType.PointerUp };
            entry1.callback.AddListener((eventData) => { p.pMove.MoveButtonUp(); });
            triggerUp.triggers.Add(entry1);

            p.ui.rightSideBut.GetComponent<Button>().onClick.AddListener(p.CmdEndTurn);

            ColorBlock cb = p.ui.rightSideBut.colors;
            cb.normalColor = Color.green;
            p.ui.rightSideBut.colors = cb;

            ColorBlock cb1 = p.ui.leftSideBut.colors;
            cb1.normalColor = Color.red;
            p.ui.leftSideBut.colors = cb1;

            //speed = -speed;
        }
        else{
            Debug.Log("add");

            EventTrigger triggerDown = p.ui.rightSideBut.GetComponentInParent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry{ eventID = EventTriggerType.PointerDown };
            entry.callback.AddListener((eventData) => { p.pMove.MoveButtonDown(); });
            triggerDown.triggers.Add(entry);

            EventTrigger triggerUp = p.ui.rightSideBut.GetComponentInParent<EventTrigger>();
            EventTrigger.Entry entry1 = new EventTrigger.Entry{ eventID = EventTriggerType.PointerUp };
            entry1.callback.AddListener((eventData) => { p.pMove.MoveButtonUp(); });
            triggerUp.triggers.Add(entry1);

            p.ui.leftSideBut.GetComponent<Button>().onClick.AddListener(p.CmdEndTurn);

            ColorBlock cb = p.ui.rightSideBut.colors;
            cb.normalColor = Color.red;
            p.ui.rightSideBut.colors = cb;

            ColorBlock cb1 = p.ui.leftSideBut.colors;
            cb1.normalColor = Color.green;
            p.ui.leftSideBut.colors = cb1;
        }
    }*/
}
