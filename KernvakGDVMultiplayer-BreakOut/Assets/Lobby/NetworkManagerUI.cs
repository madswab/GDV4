using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking.Match;
using UnityEngine.SceneManagement;

public class NetworkManagerUI : MonoBehaviour {

    public static NetworkManagerUI single;

    public GameObject startMenu;
    public GameObject createMatchMenu;
    public GameObject matchListMenu;
    public GameObject lobbyMenu;
    public GameObject startGameButton;

    public Transform matchesListGrid;
    public Transform lobbyPlayersGrid;

    public Text title;
    public Text nickName;
    public InputField gameNameInputField;

    private GameObject matchUIPrefab;
    private GameObject lobbyPlayerUIPrefab;
    private List<GameObject> lobbyPlayersIU = new List<GameObject>();
    private List<GameObject> matchSlots = new List<GameObject>();


    private void Awake(){
        if (single != null){
            Destroy(this);
        }
        else{
            single = this;
        }
    }

    private void Start(){
        startMenu.SetActive(true);
        createMatchMenu.SetActive(false);
        matchListMenu.SetActive(false);
        lobbyMenu.SetActive(false);

        title.text = "Select";
        nickName.text = Login.instance.nickName;

        matchUIPrefab = Resources.Load("MatchTemplate") as GameObject;
        lobbyPlayerUIPrefab = Resources.Load("LobbyPlayerUI") as GameObject;

        startGameButton.SetActive(false);
    }

    public void OpenSearchMatch(){
        startMenu.SetActive(false);
        title.text = "Join Match";
        matchListMenu.SetActive(true);
        createMatchMenu.SetActive(false);
        NetworkManagerV2.single.SearchForMatch();
    }

    //public void CreateMatch(){
    //    NetworkManagerV2.single.CreateMatch();
    //}

    public void OpenCreateMatchMenu(){
        startMenu.SetActive(false);
        matchListMenu.SetActive(false);
        createMatchMenu.SetActive(true);
        title.text = "Create Match";
        NetworkManagerV2.single.InitMatch();
        gameNameInputField.text = NetworkManagerV2.single.currentMatch.matchName;
    }

    public void OpenLobbyMenu(){
        startMenu.SetActive(false);
        createMatchMenu.SetActive(false);
        matchListMenu.SetActive(false);
        lobbyMenu.SetActive(true);
        title.text = "Lobby";
    }

    public void UpdateGameName(string mName){
        NetworkManagerV2.single.currentMatch.matchName = mName;
    }
/*
    public void UpdateGameType(int i){
        //NetworkManagerV2.single.currentMatch.matchType = (MatchType)i;
    }

    public void UpdateGameMap(int i){
        //NetworkManagerV2.single.currentMatch.map = i;
    }

    public void UpdateGameMaxPlayers(string i){
        //NetworkManagerV2.single.currentMatch.maxPlayers = i;
    }
*/
    public void AddMatchSlot(MatchInfoSnapshot mi){
        GameObject go = Instantiate(matchUIPrefab) as GameObject;
        go.transform.SetParent(matchesListGrid);
        go.transform.localScale = Vector3.one;

        MatchesUI mUI = go.GetComponent<MatchesUI>();

        string mName = "";
        //string mType = "";
        string mMap = "";
        string mStatus = "";

        NetworkManagerV2.single.ReadData(mi.name, ref mName , /*ref mType,*/ ref mMap, ref mStatus);

        mUI.nameMatch.text = mName;
        mUI.maxPlayers.text = mi.maxSize.ToString();
        mUI.amountJoined.text = mi.currentSize.ToString();
        mUI.level.text = mMap.ToString();
        mUI.snapshot = mi;
        matchSlots.Add(go);
    }

    public GameObject AddLobbyPlayerUI(string pName){
        GameObject go = Instantiate(lobbyPlayerUIPrefab) as GameObject;
        go.transform.SetParent(lobbyPlayersGrid);
        go.transform.localScale = Vector3.one;
        
        LobbyPlayerUI lpUI = go.GetComponent<LobbyPlayerUI>();
        lpUI.Init(pName);
        lobbyPlayersIU.Add(go);

        return go;
    }
/*
    public void ChangeMatchType(int i){
        MatchSettings ms = NetworkManagerV2.single.currentMatch;
        MatchType mt = NetworkManagerV2.single.IntToMatchType(i);
        ms.matchType = mt;
    }
*/
    
    public void ChangeMap(Dropdown dd){
        NetworkManagerV2.single.currentMatch.map = dd.value;
       
        //MatchSettings ms = NetworkManagerV2.single.currentMatch;
        //Debug.Log(NetworkManagerV2.single.currentMatch.map);
        //ms.map = i; //ms

    }

    public void ChangeMaxPlayers(Dropdown dd){
        NetworkManagerV2.single.currentMatch.maxPlayers = NetworkManagerV2.single.IntToMaxPlayers(dd.value);
        Debug.Log(NetworkManagerV2.single.IntToMaxPlayers(dd.value));
        //MatchSettings ms = NetworkManagerV2.single.currentMatch;
        //int mp = NetworkManagerV2.single.IntToMaxPlayers(i);
        //ms.maxPlayers = mp;

    }

    public void BackToStartMenu(){
        NetworkManagerV2.single.BackToStartMenu();
        createMatchMenu.SetActive(false);
        matchListMenu.SetActive(false);
        lobbyMenu.SetActive(false);
        startMenu.SetActive(true);
        title.text = "Select";

    }

    public void CreateMultiplayerGame(){
        NetworkManagerV2.single.CreateMatch();

    }

    public void StartMultiplayerGame(){
        NetworkManagerV2.single.StartGame();

    }

    public void ChangeAccount(){
        //SceneManager.LoadScene("Account");
        NetworkManagerV2.single.ServerChangeScene("Account");

    }

    public void HighScore(){
        //SceneManager.LoadScene("HighScore");
        NetworkManagerV2.single.ServerChangeScene("HighScore");


    }
}
