using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HighScore : MonoBehaviour {

    public int amountScores;
    public GameObject scorePanel;
    public Text gamesPlayedAmound;
    public InfoPlayers playersHighScore;

    private string dateOne;
    private string dateTwo;
    private double daysBackDateOne;
    private double daysBackDateTwo = 1;

    private string RecentGamesURL = "http://studenthome.hku.nl/~bas.dewaal/Database/RecentGames.php?";
    private string HighScoreURL = "http://studenthome.hku.nl/~bas.dewaal/Database/Highscorelijst.php?";

    private void Awake(){
        StartCoroutine(GetInfoFromDBHighscoreGame());
        StartCoroutine(GetTotaalGamesPlayedFromDB());

    }

    public void BacktoLobby(){
        NetworkManagerV2.single.ServerChangeScene("Lobby");
    }

    public void DropdownChangeDate(Dropdown dropdown){
        switch (dropdown.value){
            case 0:
                daysBackDateOne = -1;
                break;
            case 1:
                daysBackDateOne = -7;
                break;
            case 2:
                daysBackDateOne = -30; /// ?
                break;
            case 3:
                StartCoroutine(GetInfoFromDBHighscoreGame());
                break;
        }

        if (dropdown.value == 3){
            return;
        }

        dateOne = System.DateTime.Now.AddDays(daysBackDateOne).ToString("yyyy-MM-dd");
        dateTwo = System.DateTime.Now.AddDays(daysBackDateTwo).ToString("yyyy-MM-dd");

        StartCoroutine(GetInfoFromDBBetweenDates());
    }

    private IEnumerator GetTotaalGamesPlayedFromDB(){
        WWWForm form = new WWWForm();
        form.AddField("Total", "get");
        form.AddField("Game", Login.instance.gameName);

        WWW www = new WWW(HighScoreURL + "PHPSESSID=" + Login.instance.sessionID, form);

        yield return www;

        gamesPlayedAmound.text = www.text;

    } 

    private IEnumerator GetInfoFromDBHighscoreGame(){
        WWWForm form = new WWWForm();
        //form.AddField("ID", Login.instance.sessionID);
        form.AddField("Game", Login.instance.gameName);
        form.AddField("Amount", amountScores);

        WWW www = new WWW(HighScoreURL + "PHPSESSID=" + Login.instance.sessionID, form);

        yield return www;

        Debug.Log(www.text);

        string json = InfosCollection.FixJsonString(www.text);
        playersHighScore = JsonUtility.FromJson<InfoPlayers>(json);

        FillScoreList();
    }

    private IEnumerator GetInfoFromDBBetweenDates(){
        WWWForm form = new WWWForm();
        //form.AddField("ID", Login.instance.sessionID);
        form.AddField("dateFirst", dateOne.ToString());
        form.AddField("dateSecond", dateTwo.ToString());
        form.AddField("Game", Login.instance.gameName);
        form.AddField("Amount", amountScores);

        WWW www = new WWW(RecentGamesURL + "PHPSESSID=" + Login.instance.sessionID, form);

        yield return www;

        string json = InfosCollection.FixJsonString(www.text);
        playersHighScore = JsonUtility.FromJson<InfoPlayers>(json);

        FillScoreList();
    }

    private void FillScoreList(){
        foreach (Transform item in scorePanel.transform){
            Destroy(item.gameObject);
        }

        foreach (var item in playersHighScore.Items){
            GameObject highscorePlayer = Instantiate(Resources.Load("HighScorePlayerInfo")) as GameObject;
            highscorePlayer.transform.SetParent(scorePanel.transform);
            highscorePlayer.GetComponent<HighScorePlayerInfo>().SetInfo(item.Nickname, item.Date.Substring(0, 10), item.Score);
        }
    }

}


[System.Serializable]
public class InfoPlayers{

    public PlayerInfo[] Items;

    public static string FixJsonString(string json){
        json = "{\"Items\":" + json + "}";
        return json;
    }

    [System.Serializable]
    public struct PlayerInfo{

        public string Nickname;
        public string Score;
        public string Date;

    }

}
