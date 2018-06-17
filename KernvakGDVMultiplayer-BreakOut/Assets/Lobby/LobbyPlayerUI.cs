using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPlayerUI : MonoBehaviour {

    public Text playerName;

    public void Init(string pName){
        playerName.text = pName;
    }
}
