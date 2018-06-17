using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisShader : MonoBehaviour{

    public float value = 1.0f;
    public bool isRunning = false;
    public Material dissolveMaterial = null;
    public float timeScale = 1.0f;

    void Start(){
        dissolveMaterial = GetComponent<Renderer>().material;
    }

    public void Reset(){
        value = 1.0f;
        dissolveMaterial.SetFloat("_DissolveValue", value);
    }

    public void TriggerDissolve(Vector3 hitPoint){
        value = 1.0f;
        dissolveMaterial.SetVector("_HitPos", (new Vector4(hitPoint.x, hitPoint.y, hitPoint.z, 1.0f)));
        isRunning = true;
    }

    void Update(){
        if (isRunning && dissolveMaterial != null){
            value = Mathf.Max(-0.06f, value - Time.deltaTime * timeScale);
            dissolveMaterial.SetFloat("_DissolveValue", value);
        }

    }
}