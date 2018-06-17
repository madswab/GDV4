using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetShader : MonoBehaviour
{

    public Vector3 point;
    public bool didHit = false;
    public DisShader targetEffect;

    void Update(){
        if (didHit){
            didHit = false;
            targetEffect.TriggerDissolve(point);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if (collision.collider.gameObject.GetComponent<DisShader>()){
            targetEffect = collision.collider.gameObject.GetComponent<DisShader>();
            if (targetEffect != null){
                didHit = true;
                point = collision.GetContact(0).point;
                targetEffect.Reset();
            }
        }
    }

}
