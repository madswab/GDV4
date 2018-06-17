using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class MultiplayerLevelLoader : NetworkBehaviour {

    [SyncVar]
    public string levelName;

    private bool levelLoaded;


    private void Start () {
        levelLoaded = false;

        if (isServer){
            MatchSettings matchSets = NetworkManagerV2.single.currentMatch;
            Debug.Log("level name changed");
            levelName = NetworkManagerV2.single.IntToMapName(matchSets.map.ToString());
        }

        if (isLocalPlayer){
            Init();
        }
	}

    private void Init(){
        if (string.IsNullOrEmpty(levelName)){
            Debug.Log("level name is null");
            return;
        }

        Debug.Log(levelName);
        StartCoroutine(LoadScene());
    }

    private IEnumerator LoadScene(){

        //if (isServer){
        //    levelName = "Headless";
        //}

        yield return SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
        levelLoaded = true;
        Debug.Log("level loaded succeded");
    }

}
