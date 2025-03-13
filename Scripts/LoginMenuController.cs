using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginMenu : MonoBehaviour
{
    public GameObject loginPanel, signUpPanel, scoreboardPanel, notificationPanel, DatabaseController;
    public TMP_InputField loginUsername, loginPassword, signupUsername, signupPassword, signupConfPassword;
    public TextMeshProUGUI notifTitleText, notifMessageText;

    public void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void OpenLoginPanel(){
        loginPanel.SetActive(true);
        scoreboardPanel.SetActive(true);
        signUpPanel.SetActive(false);
    }

    public void OpenSignUpPanel(){
        loginPanel.SetActive(false);
        scoreboardPanel.SetActive(false);
        signUpPanel.SetActive(true);
    }

    public void LoginUser(){
        if(string.IsNullOrWhiteSpace(loginUsername.text) || string.IsNullOrWhiteSpace(loginPassword.text)){
            ShowErrorNotificationMessage("ERROR", "BLANK PASSWORD OR USERNAME");
            return;
        }

        else if(loginUsername.text.Length < 4  || loginUsername.text.Length > 11)
        {
            ShowErrorNotificationMessage("ERROR", "USERNAME TOO LONG OR SHORT, NEEDS TO BE 4-11 CHARACTERS LONG");
            return;
        }
        else if(loginPassword.text.Length < 8 || loginPassword.text.Length >= 20)
        {
            ShowErrorNotificationMessage("ERROR", "PASSWORD TOO LONG OR SHORT, NEEDS TO BE 8-20 CHARACTERS LONG");
            return;
        }

        bool loginSuccess = DatabaseController.GetComponent<DatabaseController>().LoginUser(loginUsername.text, loginPassword.text);

        if(loginSuccess){
            SceneManager.LoadScene("MainMenu");
        }
        else{
            ShowErrorNotificationMessage("ERROR", "USERNAME/PASSWORD INVALID, USER NOT FOUND, CHECK SPELLING AND MAKE SURE YOUR ACCOUNT IS CREATED");
        }
    }

    public void SignUpUser(){
        if(string.IsNullOrWhiteSpace(signupUsername.text) || string.IsNullOrWhiteSpace(signupPassword.text) || string.IsNullOrWhiteSpace(signupConfPassword.text)){
            ShowErrorNotificationMessage("ERROR", "BLANK PASSWORD OR USERNAME");
            return;
        }
        else if(signupUsername.text.Length < 4  || signupUsername.text.Length > 11)
        {
            ShowErrorNotificationMessage("ERROR", "USERNAME TOO LONG OR SHORT, NEEDS TO BE 4-11 CHARACTERS LONG");
            return;
        }
        else if(signupPassword.text.Length < 8 || signupPassword.text.Length >= 20)
        {
            ShowErrorNotificationMessage("ERROR", "PASSWORD TOO LONG OR SHORT, NEEDS TO BE 8-20 CHARACTERS LONG");
            return;
        }
        else if(!signupConfPassword.text.Equals(signupPassword.text)){
            ShowErrorNotificationMessage("ERROR", "PASSWORD DOES NOT MATCH CONFIRMATION PASSWORD, BE SURE TO DOUBLE CHECK SPELLING");
            return;
        }

        bool userCreated = DatabaseController.GetComponent<DatabaseController>().CreateUser(signupUsername.text, signupPassword.text);

        if(userCreated){
            OpenLoginPanel();
        }
        else{
            ShowErrorNotificationMessage("ERROR", "USER FAILED TO BE CREATED, USERNAME IS ALREADY IN USE OR INVALID FORMAT FOR USERNAME/PASSWORD");
        }
    }

    public void ShowErrorNotificationMessage(string title, string message){
        notifTitleText.text = title;
        notifMessageText.text = message;

        notificationPanel.SetActive(true);
    }

    public void CloseErrorNotificationMessage(){
        notifTitleText.text = "";
        notifMessageText.text = "";

        notificationPanel.SetActive(false);
    }
}
