using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScorePlayerInfo : MonoBehaviour {

    private Text userName;
    private Text date;
    private Text score;


    private void Awake(){
        userName = transform.GetChild(0).GetComponent<Text>();
        date = transform.GetChild(1).GetComponent<Text>();
        score = transform.GetChild(2).GetComponent<Text>();

    }

    public void SetInfo(string userName, string date, string score){
        this.userName.text = userName;
        this.date.text = date;
        this.score.text = score;

    }



}
