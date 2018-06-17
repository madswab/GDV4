using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class FormulierAccount : MonoBehaviour {

    public InfosCollection playerInfo;
    public string json;

    public InputField inputEmail;
    public InputField inputPassword;
    public InputField inputNickname;

    public Dropdown dropdownDay;
    public Dropdown dropdownMonth;
    public Dropdown dropdownYear;
    public Dropdown dropdownSex;

    private int maxDays = 31;
    private int maxMonths = 12;
    private int minYears = 1910;
    private Login login;

    private string formulierAccountURL = "http://studenthome.hku.nl/~bas.dewaal/Database/Formulier.php?";

    public void Awake(){
        FillDropdownMenus();
        login = GameObject.Find("LoginManager").GetComponent<Login>();

        if (login.loggedIn){
            StartCoroutine(GetAccount());
        }

    }

    public void ConfirmButton(){
        StartCoroutine(SetAccount());

    }

    public void BackButton(){
        if (Login.instance.loggedIn){
            NetworkManagerV2.single.ServerChangeScene("Lobby");
        }
        else{
            SceneManager.LoadScene("Login");

        }

    }

    public IEnumerator GetAccount(){
        WWWForm form = new WWWForm();
        form.AddField("GetSet", "Get");
        form.AddField("ID", login.sessionID);
        WWW www = new WWW(formulierAccountURL + "PHPSESSID=" + login.sessionID, form);

        yield return www;

        Debug.Log(www.text);

        json = InfosCollection.FixJsonString(www.text);
        playerInfo = JsonUtility.FromJson<InfosCollection>(json);

        inputEmail.text = playerInfo.Items[0].Email;
        inputPassword.text = playerInfo.Items[0].Password;
        inputNickname.text = playerInfo.Items[0].Nickname;
        dropdownDay.value = int.Parse(playerInfo.Items[0].Birthday.Substring(8, 2)) - 1;
        dropdownMonth.value = int.Parse(playerInfo.Items[0].Birthday.Substring(5, 2)) - 1;
        dropdownYear.value = int.Parse(playerInfo.Items[0].Birthday.Substring(0, 4)) - minYears;
        dropdownSex.value = int.Parse(playerInfo.Items[0].Sex);

        //{"Items":[{"Id":"1","Email":"test@test.com","Birthday":"2018-05-14","Password":"1","Sex":"0","Started_at":"2018-05-14 00:00:00","Nickname":"UserTest1"}]}

    }
    
    public IEnumerator SetAccount(){
        if (CheckForEmteyFields()){
            WWWForm form = new WWWForm();
            if (login == false && login.sessionID != null){
                form.AddField("ID", login.sessionID);
            }
            form.AddField("GetSet", "Set");
            form.AddField("Email", inputEmail.text);
            form.AddField("Password", inputPassword.text);
            form.AddField("Birthday", int.Parse(dropdownYear.options[dropdownYear.value].text) + "-" + int.Parse(dropdownMonth.options[dropdownMonth.value].text) + "-" + int.Parse(dropdownDay.options[dropdownDay.value].text));
            form.AddField("Sex", dropdownSex.value);
            form.AddField("Nickname", inputNickname.text);

            WWW www = new WWW(formulierAccountURL, form);

            yield return www;


            Debug.Log(www.text);
            if (www.text == "Success" && Login.instance.loggedIn){
                //SceneManager.LoadScene("Lobby");
                NetworkManagerV2.single.ServerChangeScene("Lobby");
            }
            else if (www.text == "Success"){
                SceneManager.LoadScene("Login");

            }
        }
    }

    private bool CheckForEmteyFields(){
        if (inputEmail.text != "" && inputEmail.text != null &&
        inputPassword.text != "" && inputPassword.text != null &&
        inputNickname.text != "" && inputNickname.text != null){
            return true;
        }

        return false;
    }
    
    private void FillDropdownMenus(){
        dropdownDay.ClearOptions();
        List<string> days = new List<string>();
        for (int i = 1; i <= maxDays; i++){
            days.Add(i.ToString());
        }
        dropdownDay.AddOptions(days);


        dropdownMonth.ClearOptions();
        List<string> months = new List<string>();
        for (int i = 1; i <= maxMonths; i++){
            months.Add(i.ToString());
        }
        dropdownMonth.AddOptions(months);


        dropdownYear.ClearOptions();
        List<string> years = new List<string>();
        for (int i = minYears; i <= System.DateTime.Today.Year; i++){
            years.Add(i.ToString());
        }
        dropdownYear.AddOptions(years);
    }
}

[System.Serializable]
public class InfosCollection{

    public PlayerInfo[] Items;
    
    public static string FixJsonString(string json){
        json = "{\"Items\":" + json + "}";
        return json;
    }

    [System.Serializable]
    public struct PlayerInfo{

        public string Id;
        public string Email;
        public string Birthday;
        public string Password;
        public string Sex;
        public string Started_at;
        public string Nickname;

    }

}