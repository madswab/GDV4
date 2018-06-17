using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    public Text leftText;
    public Text richtText;

    public Button leftSideBut;
    public Button rightSideBut;
    public GameObject gameOver;
    public Text lives;
    public Text score;

    public PlayerV2 player;

    private void Start(){
        //if (isServer){
            //player = GetComponent<PlayerV2>();
            StartCoroutine(AddButtonFunctions());
            richtText.text = player.playerId.ToString();
        //}
    }

    public void MoveButtonDown(){
        player.MovePanelBool = true;
    }

    public void MoveButtonUp(){
        player.MovePanelBool = false;
    }

    public IEnumerator AddButtonFunctions(){
        yield return new WaitForSeconds(0.35f);

        if (player.playerControlsLeftDirection){
            Debug.Log("addplayerControlsLeftDirection");

            EventTrigger triggerDown = leftSideBut.GetComponentInParent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry{ eventID = EventTriggerType.PointerDown };
            entry.callback.AddListener((eventData) => { MoveButtonDown(); });
            triggerDown.triggers.Add(entry);

            EventTrigger triggerUp = leftSideBut.GetComponentInParent<EventTrigger>();
            EventTrigger.Entry entry1 = new EventTrigger.Entry{ eventID = EventTriggerType.PointerUp };
            entry1.callback.AddListener((eventData) => { MoveButtonUp(); });
            triggerUp.triggers.Add(entry1);

            rightSideBut.GetComponent<Button>().onClick.AddListener(player.ButNextTurn);
/*
            ColorBlock cb = rightSideBut.colors;
            cb.normalColor = Color.green;
            rightSideBut.colors = cb;

            ColorBlock cb1 = leftSideBut.colors;
            cb1.normalColor = Color.red;
            leftSideBut.colors = cb1;
*/
            leftText.text = "<";
            richtText.text = "";

            player.speed = -player.speed;
        }
        else{
            Debug.Log("add");

            EventTrigger triggerDown = rightSideBut.GetComponentInParent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry{ eventID = EventTriggerType.PointerDown };
            entry.callback.AddListener((eventData) => { MoveButtonDown(); });
            triggerDown.triggers.Add(entry);

            EventTrigger triggerUp = rightSideBut.GetComponentInParent<EventTrigger>();
            EventTrigger.Entry entry1 = new EventTrigger.Entry{ eventID = EventTriggerType.PointerUp };
            entry1.callback.AddListener((eventData) => { MoveButtonUp(); });
            triggerUp.triggers.Add(entry1);

            leftSideBut.GetComponent<Button>().onClick.AddListener(player.ButNextTurn);
/*
            ColorBlock cb = rightSideBut.colors;
            cb.normalColor = Color.red;
            rightSideBut.colors = cb;

            ColorBlock cb1 = leftSideBut.colors;
            cb1.normalColor = Color.green;
            leftSideBut.colors = cb1;
*/
            leftText.text = "";
            richtText.text = ">";
        }
    }

}