using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameOver : NetworkBehaviour {

    [SyncVar(hook = "SetLives")]
    public int lives;

    public GameObject gameOverScreen;

    private string ScoreInputURL = "http://studenthome.hku.nl/~bas.dewaal/Database/Game-Choice.php?";


    public void SetLives(int i){
        lives = i;
        if (lives == 0 && isLocalPlayer){
            Debug.Log("GO");
            StartCoroutine(BackToLobby());
            //RpcSetGameOver();
        }

    }

    public IEnumerator BackToLobby(){
        SetGameOver();
        yield return null;

    }

    //[Command]
    public void SetGameOver(){
        gameOverScreen.SetActive(true);
       // if (isLocalPlayer)
       // {
            SendScore();

       // }
    }

    public IEnumerator SendScore()
    {
        WWWForm form = new WWWForm();
        form.AddField("Score", Login.instance.score);

        WWW www = new WWW(ScoreInputURL + "PHPSESSID=" + Login.instance.sessionID, form);

        yield return www;

        Debug.Log(www.text);

        yield return new WaitForSeconds(2f);

        NetworkManagerV2.single.ServerChangeScene("Lobby");

    }
}
