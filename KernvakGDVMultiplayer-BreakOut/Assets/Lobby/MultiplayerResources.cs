using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerResources : MonoBehaviour {

    public string[] scenes;
    public static MultiplayerResources single;

    private void Awake(){
        single = this;
    }


}
