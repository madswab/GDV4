using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Login : MonoBehaviour {

    public static Login instance;

    public string sessionID;
    public string nickName;
    public string inputEmail;
    public string inputPassword;
    public bool loggedIn;
    public string gameName = "MulitplayerBreakout";
    public int score;

    private string LoginURL = "http://studenthome.hku.nl/~bas.dewaal/Database/Login.php?";

    private void Start(){
        DontDestroyOnLoad(this);

        if (instance == null){
            instance = this;
        }
        else{
            Destroy(this);
        }

    }

    public IEnumerator LoginToDB(){
        if (inputEmail != "" && inputPassword != ""){
            WWWForm form = new WWWForm();
            form.AddField("Email", inputEmail);
            form.AddField("Password", inputPassword);
            form.AddField("Game", gameName);

            WWW www = new WWW(LoginURL, form);

            yield return www;
            Debug.Log(www.text);

            if (www.text.Contains("Wrong")){
                LoginSceneForm.WrongLogin(true);
            }
            else{
                Debug.Log("login Success");
                LoginSceneForm.WrongLogin(false);
                string[] splitString = www.text.Split('/');
                sessionID = splitString[0];
                nickName = splitString[1];
                loggedIn = true;
                SceneManager.LoadScene("Lobby");
            }
        }
        else{
            LoginSceneForm.WrongLogin(true);

        }
    }
    



}
