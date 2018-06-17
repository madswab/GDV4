using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class MatchesUI : MonoBehaviour {

    public Text nameMatch;
    public Text maxPlayers;
    public Text amountJoined;
    public Text level;

    public MatchInfoSnapshot snapshot;


    public void PressButton(){
        if (snapshot.currentSize < snapshot.maxSize){
            NetworkManagerV2.single.TryToJoinMatch(snapshot);
        }
        else{
            Debug.Log("max users in match");
        }
    }

}
