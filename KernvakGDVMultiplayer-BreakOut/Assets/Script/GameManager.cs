using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour {

    public static GameManager instance;

    public GameObject ball;
    public GameObject panel;
    public float yPosPlayer;
    public int currentPlayerTurn = 0;

    public List<Player> players = new List<Player>();

    private Ball curBall;
    private bool playersReady = false;

  
}

/*
  private void Awake(){
        if (instance == null){
            instance = this;
        }
        else{
            Destroy(this);
        }

        yPosPlayer = panel.transform.position.y;
    }

    private void Start(){
        StartCoroutine(Delay());

    }

    private void Update(){
        if (curBall != null){
            curBall.ServerMovement();
        }
    }

    private IEnumerator Delay(){    
        while (playersReady == false){

            int playersR = 0;
            foreach (var item in players){
                if (item.ready){
                    playersR++;
                }
            }

            if (players.Count == playersR && players.Count > 1 && NetworkServer.connections.Count == playersR){ /// ---
                playersReady = true;
            }

            yield return new WaitForEndOfFrame();
        }

        GameObject b = Instantiate(ball);
        NetworkServer.Spawn(b);
        curBall = b.GetComponent<Ball>();
        curBall.ServerStart();
    }

    public void AddPlayer(Player player){
        players.Add(player);     
    }

    public static bool IsTurn(int id){
        //if (instance.currentPlayerTurn >= instance.players.Count){ // When they leave 
            //instance.currentPlayerTurn = 0;
            //instance.CheckPlayerIds();
        //}
        return instance.players[instance.currentPlayerTurn].playerId == id;
    }

    public static int Register(Player p){
        instance.players.Add(p);
        Debug.Log("Registered player " + p);
       // p.ButInit();
        return instance.players.Count;
    }

    public static void NextTurn(){
        if (++instance.currentPlayerTurn >= instance.players.Count){
            instance.currentPlayerTurn = 0;
        }
    }

    */
