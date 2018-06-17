using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchSettingsUI : MonoBehaviour {

    public Dropdown dropdown;

    //public MatchSettingsUIType type;


    public void ValueCahnge(){
        switch (true){
            /*case MatchSettingsUIType.map:
                NetworkManagerUI.single.ChangeMap(dropdown.value);
                break;
            case MatchSettingsUIType.maxPlayers:
                NetworkManagerUI.single.ChangeMaxPlayers(dropdown.value);
                break;
*/
            default:
                break;
        }
    }

}
