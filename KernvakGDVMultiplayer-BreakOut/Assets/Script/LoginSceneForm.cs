using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginSceneForm : MonoBehaviour {


    public Text emailInputField;
    public Text passwordInputField;

    public GameObject warningObj;
    public static GameObject warning;

    public void Start(){
        warning = warningObj;

    }

    public void CreateAccount(){
        SceneManager.LoadScene("Account");

    }

    public void LoginButton(){
        Login.instance.inputEmail = emailInputField.text;
        Login.instance.inputPassword = passwordInputField.text;

        Login.instance.StartCoroutine(Login.instance.LoginToDB());

    }

    public static void WrongLogin(bool OnOrOff){
        warning.SetActive(OnOrOff);

    }
}
