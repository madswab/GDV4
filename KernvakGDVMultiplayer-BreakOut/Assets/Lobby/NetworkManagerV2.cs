using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class NetworkManagerV2 : NetworkLobbyManager {

    public static NetworkManagerV2 single;

    public bool host;
    public MatchSettings currentMatch;
    public List<LobbyPlayerInfo> lobbyPlayers = new List<LobbyPlayerInfo>();

    private void Awake(){
        if (single != null){
            Destroy(this);
        }
        else{
            single = this;
        }
    }

    private void Start(){

    }


    public void InitMatch(){
        currentMatch = new MatchSettings();
        currentMatch.matchName = "Game";
        currentMatch.map = 0;
        currentMatch.maxPlayers = 2;
        currentMatch.status = 4;
    }

    public void SearchForMatch(){
        StartMatchMaker();
        matchMaker.ListMatches(0, 20, "", true, 0, 0, ReturnMatch);
    }

    public void ReturnMatch(bool success, string extendedInfo, List<MatchInfoSnapshot> matches){
        Debug.Log(matches.Count + " matches found");

        for (int i = 0; i < matches.Count; i++){
            NetworkManagerUI.single.AddMatchSlot(matches[i]);
        }
    }

    public void CreateMatch(){
        StartMatchMaker();

        string data = currentMatch.matchName;
        data += ".M.";
        data += currentMatch.map.ToString();
        data += ".S.";
        data += currentMatch.status.ToString();

        matchMaker.CreateMatch(data, (uint)currentMatch.maxPlayers, true, "", "", "", 0, 0, MatchCreated);
    }

    public void ReadData(string data, ref string n, /*ref string t,*/ ref string m, ref string s){
        string[] name = data.Split(new string[] { ".M." }, StringSplitOptions.RemoveEmptyEntries);
        n = name[0];

        string data2 = name[1];
        string[] map = data2.Split(new string[] { ".S." }, StringSplitOptions.RemoveEmptyEntries);

        m = IntToMapName(map[0]);
        s = IntToState(map[1]);

    }
/*
    MatchType StringToMatchType(string mType){
        int type = 0;
        if (Int32.TryParse(mType, out type)){

        }
        return (MatchType)type;
    }
*/
    public string IntToMapName(string m){
        int map = 0;
        if (Int32.TryParse(m, out map)){

        }

        string retVal = "level1";
        retVal = MultiplayerResources.single.scenes[map];

/*
        switch (mType){
            case MatchType.deathmatch:
            retVal = MultiplayerResources.single.scenes[map];
                break;
            case MatchType.e:
                retVal = "level2";
                break;
            default:
                break;
        }
*/
        return retVal;
    }

    public string IntToState(string s){
        int stat = 0;
        if (Int32.TryParse(s, out stat)){
            if (stat == 0){
                return "In lobby";
            }
            else{
                return "In game";
            }
        }

        return "In lobby";
    }
/*
    public MatchType IntToMatchType(int i){
        return (MatchType)i;
    }
*/
    public int IntToMaxPlayers(int i){
        switch (i){
            case 0:
                return 2;
            case 1:
                return 3;
            default:
                return 2;
        }
    }

    private void MatchCreated(bool success, string extendedInfo, MatchInfo matchInfo){
        if (success){
            host = true;
            NetworkManagerUI.single.OpenLobbyMenu();
            Debug.Log("created");
            NetworkServer.Listen(matchInfo, 9000);
            StartHost(matchInfo);
            
        }
    }

    public void TryToJoinMatch(MatchInfoSnapshot m){
        matchName = m.name;//
        matchSize = (uint)m.currentSize;//
        matchMaker.JoinMatch(m.networkId, "", "", "", 0, 0, MatchJoin);
    }

    private void MatchJoin(bool success, string extendedInfo, MatchInfo matchInfo){
        if (success){
            host = false;
            StartClient(matchInfo);
            NetworkManagerUI.single.OpenLobbyMenu();
            Debug.Log("Joined");          
        }
    }

    public void BackToStartMenu(){//
        StopClient();
        StopHost();
        StopMatchMaker();
        StartCoroutine(CheckRemovedPlayers());
        lobbyPlayers.Clear();
    }

    public override void OnClientDisconnect(NetworkConnection conn){
        base.OnClientDisconnect(conn);
    }

    public override void OnServerDisconnect(NetworkConnection conn){
        base.OnServerDisconnect(conn);
        StartCoroutine(CheckRemovedPlayers());
    }

    private IEnumerator CheckRemovedPlayers(){
        yield return new WaitForSeconds(1);

        LobbyPlayerInfo toRemove = null;
        for (int i = 0; i < lobbyPlayers.Count; i++){
            if (lobbyPlayers[i].lobbyPlayer == null){
                if (lobbyPlayers[i].UiIcon.gameObject.activeInHierarchy){
                    Destroy(lobbyPlayers[i].UiIcon.gameObject);
                }
                toRemove = lobbyPlayers[i];
            }
        }

        if (lobbyPlayers.Contains(toRemove)){
            lobbyPlayers.Remove(toRemove);
        }
    }

    public void AddLobbyPlayer(LobbyPlayer lp){
        for (int i = 0; i < lobbyPlayers.Count; i++){
            if (lobbyPlayers[i].lobbyPlayer.netId == lp.netId){
                return;
            }
        }

        LobbyPlayerInfo inf = new LobbyPlayerInfo();
        inf.lobbyPlayer = lp;
        inf.UiIcon = NetworkManagerUI.single.AddLobbyPlayerUI(lp.playerName).GetComponent<LobbyPlayerUI>();
        inf.UiIcon.Init(inf.lobbyPlayer.playerName);
        lobbyPlayers.Add(inf);

        StartCoroutine(DelayBroadcastPlayersName());
    }

    private IEnumerator DelayBroadcastPlayersName(){
        yield return new WaitForSeconds(0.3f);

        foreach (LobbyPlayerInfo item in lobbyPlayers){
            if (item.lobbyPlayer.isLocalPlayer){
                item.lobbyPlayer.CmdBroadcastPlayername(item.lobbyPlayer.netId.Value);
            }
        }
    }

    public void StartGame(){
        foreach (LobbyPlayerInfo item in lobbyPlayers){
            if (item.lobbyPlayer.isLocalPlayer){
                item.lobbyPlayer.SendReadyToBeginMessage();
            }
        }
    }

    public LobbyPlayerInfo GetLobbyPlayer(uint playerID){
        for (int i = 0; i < lobbyPlayers.Count; i++){
            if (lobbyPlayers[i].lobbyPlayer.netId.Value == playerID){
                return lobbyPlayers[i];
            }
        }

        return null;
    }

}


public enum MatchSettingsUIType{
    map, maxPlayers
}

public class MatchSettings{
    public string matchName;
    public int map;
    public int maxPlayers;
    public int status;
}

[Serializable]
public class LobbyPlayerInfo{
    public LobbyPlayer lobbyPlayer;
    public LobbyPlayerUI UiIcon;
}