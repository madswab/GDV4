using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerV2 : NetworkBehaviour {

    [SyncVar]
    public int playerId = -1;
    [SyncVar]
    public bool playerControlsLeftDirection;

    public PlayerUI ui;

    [SyncVar]
    public bool MovePanelBool;
    [SyncVar]
    public float speed = 10f;

    [SyncVar]
    public bool myTurn;

    private string ScoreInputURL = "http://studenthome.hku.nl/~bas.dewaal/Database/Game-Choice.php?";


    private void Start(){
        StartCoroutine(DelayAddToGame());
        ui = transform.GetChild(0).GetComponent<PlayerUI>();
        Login.instance.score = 0;
    }

    private void Update(){
        if (isLocalPlayer){

            if (myTurn == false){
                CmdCheckMyTurn();
            }

            if (Input.GetKeyDown(KeyCode.B)){
                Debug.Log(playerId + "pressed B");
                StartCoroutine(SendScore());
            }

            if (myTurn){
                if (MovePanelBool){
                    CmdMovePanel();
                }

                if (Input.GetKeyDown(KeyCode.A)){
                    Debug.Log(playerId + "pressed A");
                    CmdEndTurn();
                }

                if (Input.GetKey(KeyCode.D) ){
                    Debug.Log(playerId + "pressed d");
                    CmdMovePanel();
                }
            }
        }

    }

    public IEnumerator DelayAddToGame(){
        yield return new WaitForSeconds(0.25f);
        if (isServer){
            GameManagerV2.AddPlayer(this);
            CmdEndTurn();
        }

        if (isLocalPlayer){
            transform.GetChild(0).gameObject.SetActive(true);
            //ui = transform.GetChild(0).GetComponent<PlayerUI>();

        }
    }

    [ClientRpc]
    public void RpcYourTurn(bool isYourTurn){
        if (isYourTurn && isLocalPlayer){
            //GameManagerV2.EnablePlayHandButtons(this);
            myTurn = true;
        }
        else{
            //GameManagerV2.ClientDisableAllButtons(this);
            myTurn = false;
        }
    }

    [Command]
    private void CmdCheckMyTurn(){
        myTurn = GameManagerV2.IsTurn(playerId);
    }


    [Command]
    private void CmdMovePanel(){
        GameManagerV2.MovePanel(speed);
    }


    [Command]
    public void CmdEndTurn(){
        if (GameManagerV2.IsTurn(playerId)){
            GameManagerV2.NextPlayer();
        }
    }

    public void ButNextTurn(){
        CmdEndTurn();
    }

    public IEnumerator SendScore(){
        WWWForm form = new WWWForm();
        form.AddField("Score", Login.instance.score);

        WWW www = new WWW(ScoreInputURL + "PHPSESSID=" + Login.instance.sessionID, form);

        yield return www;

        Debug.Log(www.text);

        yield return new WaitForSeconds(2f);

        NetworkManagerV2.single.BackToStartMenu();
        NetworkManagerV2.single.ServerChangeScene("Lobby");

    }

}
