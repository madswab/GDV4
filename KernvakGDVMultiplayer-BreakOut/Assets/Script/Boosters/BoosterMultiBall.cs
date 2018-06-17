using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterMultiBall : MonoBehaviour {

    public float speed = 3;


    private void Update(){
        Boosters.BoosterMovement(transform, speed);

    }

    private void OnCollisionEnter2D(Collision2D collision){
        if (collision.transform.root.tag == "Player"){
            Ball[] b = GameManagerV2.single.balls.ToArray();
            foreach (var item in b){
                item.MulitBallsBooster();
            }
            //GameManagerV2.single.balls.SpeedBoost(extraBallSpeed);
            Destroy(gameObject);
        }

    }
}
