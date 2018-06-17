using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Boosters : NetworkBehaviour {

    public static System.Action<Vector2> dropBooster;

    public GameObject[] boosters;

    private void Start(){
        dropBooster += InstantiateRandomBooster;
    }

    private void Update(){
        if (Input.GetKeyDown(KeyCode.B)){
            RandomBooster(new Vector3(0,0,0));
        }
    }

    public static void RandomBooster(Vector2 pos){
        if (dropBooster != null){
            dropBooster(pos);
        }
    }

    private void InstantiateRandomBooster(Vector2 pos){
        if (boosters.Length > 0){
            GameObject b = Instantiate(boosters[Random.Range(0, boosters.Length)], pos, Quaternion.identity);
            NetworkServer.Spawn(b);
        }
    }

    public static void BoosterMovement(Transform obj, float speed){
        Vector2 fallPos = obj.position;
        fallPos.y -= Time.deltaTime * speed;
        obj.position = fallPos;

        if (fallPos.y < GameManagerV2.single.panel.transform.position.y - 1){
            Destroy(obj.gameObject);
        }
    }


}
