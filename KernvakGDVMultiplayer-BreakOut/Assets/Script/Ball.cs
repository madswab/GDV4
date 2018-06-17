using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Ball : NetworkBehaviour {

    private Rigidbody2D rb;
    public float ballForce = 10f;
    public bool booster = false;
    public GameOver go;

    private bool canRemoveLive = true;
    private float maxBallForce;

    [SyncVar(hook = "OnSetScale")]
    private Vector3 scale;


    public void Start() {
        //if (!isLocalPlayer)
        //    return;
        go = GameObject.Find("Canvas").GetComponent<GameOver>();
        maxBallForce = ballForce;
        rb = GetComponent<Rigidbody2D>();
        if (booster == false){
            StartCoroutine(StartAni());
        }

    }

    public void Update() {
        if (Input.anyKeyDown){
            //MulitBallsBooster();
        }

        if (transform.position.y < GameManagerV2.single.panel.transform.position.y - 0.5f){
            if (GameManagerV2.single.balls.Count == 1 && canRemoveLive == true){
                canRemoveLive = false;
                go.lives--;

                if (go.lives - 1 >= 0){
                    StartCoroutine(StartAni());
                }
                else{
                    //GameManagerV2.single.GameOver();
                    rb.velocity = Vector2.zero;
                    RpcGO();
                }
            }
            else{
                //Destroy(gameObject);
                NetworkServer.Destroy(gameObject);
                GameManagerV2.single.balls.Remove(this);
            }

        }

    }

    [ClientRpc]
    public void RpcGO(){
        StartCoroutine(go.BackToLobby());

    }


    public void SpeedBoost(float s){
        StartCoroutine(ChangeBallSpeed(s));
    }

    public void MulitBallsBooster(){
        GameObject b = Instantiate(Resources.Load("Ball")) as GameObject;
        Ball bb = b.GetComponent<Ball>();
        bb.booster = true;
        bb.ballForce = ballForce;
        b.transform.position = transform.position;
        NetworkServer.Spawn(b);
        GameManagerV2.single.balls.Add(b.GetComponent<Ball>());
        b.GetComponent<Rigidbody2D>().AddForce(new Vector2(bb.ballForce, bb.ballForce));


    }

    private IEnumerator ChangeBallSpeed(float s){
        Vector3 lastPos = transform.position;

        yield return new WaitForSeconds(0.25f);

        Vector3 curPos = transform.position;
        Vector3 dir = Vector3.Normalize(curPos - lastPos);

        ballForce += s;
        rb.AddForce(new Vector2(dir.x * ballForce, dir.y * ballForce));

    }

    [Command]
    public void CmdSetScale(Vector3 vec){
        this.scale = vec; // This is just to trigger the call to the OnSetScale while encapsulating.
    }

    private void OnSetScale(Vector3 vec){
        this.scale = vec;
        this.transform.localScale = vec;
    }

    public IEnumerator StartAni(){
        rb.velocity = Vector2.zero;
        transform.position = new Vector3(0, -1, 0);
        float progress = 0;

        while (progress <= 1){
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, progress);
            progress += Time.deltaTime * 0.2f;
            OnSetScale(transform.localScale); 
            yield return null;
        }

        ballForce = maxBallForce;
        transform.localScale = Vector3.one;
        rb.AddForce(new Vector2(ballForce, ballForce));
        canRemoveLive = true;

    }
} 
