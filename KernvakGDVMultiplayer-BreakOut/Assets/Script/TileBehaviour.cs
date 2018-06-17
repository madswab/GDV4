using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TileBehaviour : NetworkBehaviour {

    public bool booster;
    private DisShader disValue;
    private LevelGenerator lg;
    public GameOver go;

    private void Start(){
        disValue = GetComponent<DisShader>();
        lg = transform.root.GetComponent<LevelGenerator>();
    }

    private void OnCollisionEnter2D(Collision2D col){
        if (col.gameObject.tag == "Ball"){
            StartCoroutine(Delay());
            GetComponent<BoxCollider2D>().enabled = false;
            Login.instance.score += 10;

            if (lg.levelTiles - 1 == 0){
                lg.CheckLevel();
            }
            else{
                lg.levelTiles--;
            }

            if (booster){
                Boosters.RandomBooster(transform.position);
            }
        }
    }

    private IEnumerator Delay(){ 
        while (disValue.value > 0.1f){
            yield return new WaitForSeconds(0.1f);
        }

        gameObject.SetActive(false);
        NetworkServer.UnSpawn(gameObject);
    }

}
