 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level")]
public class LevelScriptableObject : ScriptableObject {

    public Texture2D levelImage;
    public int amountBoostersInLevel;

}
