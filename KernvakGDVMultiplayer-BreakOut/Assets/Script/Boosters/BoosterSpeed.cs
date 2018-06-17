using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterSpeed : MonoBehaviour {

    public float speed = 5;
    public float extraBallSpeed = 5;

	
	private void Update () {
        Boosters.BoosterMovement(transform, speed);

	}

    private void OnCollisionEnter2D(Collision2D collision){
        if (collision.transform.root.tag == "Player"){
            foreach (var item in GameManagerV2.single.balls){
                item.SpeedBoost(extraBallSpeed);
            }
            //GameManagerV2.single.balls.SpeedBoost(extraBallSpeed);
            Destroy(gameObject);
        }

    }
}
