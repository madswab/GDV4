using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LevelGenerator : NetworkBehaviour {

    public int showLevel;

    public GameObject block;
    public LevelScriptableObject[] levels;
    public GameOver go;

    //[SyncVar(hook = "CheckLevel")]
    public int levelTiles;

    public void RemoveLevel(){
        foreach (Transform child in transform){
            DestroyImmediate(child.gameObject);
        }

        if (transform.childCount != 0){
            RemoveLevel();
        }
    }

    public void CheckLevel(){
        if (showLevel + 1 < levels.Length){
            showLevel++;
            BuildLevel();
        }
        else{
            Debug.Log("gg");
        }

    }

    public void BuildLevel(){
        levelTiles = 0;
        int amountTiles = 0;

        if (showLevel < levels.Length){
            //for (int i = 0; i < levels.Length; i++){
            GameObject level = new GameObject();
            level.name = "level " + showLevel;

            List<GameObject> allTiles = new List<GameObject>();
            int min = 0;

            for (int y = 0; y < levels[showLevel].levelImage.height; y++){
                for (int x = 0; x < levels[showLevel].levelImage.width; x++){
                    Color pixelColor = levels[showLevel].levelImage.GetPixel(x, y);

                    if (pixelColor.a != 0 && pixelColor != Color.white){
                        GameObject tile = Instantiate(block, new Vector3((x * 1.5f) - 9, y * 0.5f), Quaternion.identity); //
                        tile.transform.parent = level.transform;
                        allTiles.Add(tile);
                        tile.GetComponent<TileBehaviour>().go = go;
                        NetworkServer.Spawn(tile);
                        amountTiles++;
                    }
                }
            }

            for (int b = 0; b < levels[showLevel].amountBoostersInLevel; b++){
                int pos = Random.Range(min, allTiles.Count - 1);
                allTiles[pos].GetComponent<TileBehaviour>().booster = true;
                GameObject temp = allTiles[pos];
                allTiles[pos] = allTiles[min];
                allTiles[min] = temp;
                min++;
            }
            
            level.transform.parent = transform;
            //}
        }

        levelTiles = amountTiles;
    }

}
