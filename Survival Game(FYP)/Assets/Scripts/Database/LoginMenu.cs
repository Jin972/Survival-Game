using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.IO;
using System;
using System.Text;
using System.Net.Http;

public class LoginMenu : MonoBehaviour
{
    public GameObject loginForm;
    public GameObject registerForm;
    public GameObject loading;
    public GameObject startMenu;
    public GameObject ResetPasswordForm;

    public InputField loginEmail;
    public InputField loginPassword;
    public Text loginErrorText;

    public InputField registerEmail;
    public InputField registerPassword;
    public InputField registerConfirmPassword;
    public Text registerErrorText;

    public Text UsernameSession;

    public InputField resetPassEmail;
    public Text resetPassNotify;
    public Text resetPassErrorText;

    public GameObject Errorform;
    public Text ErrorContent;

    LoadSence loadSence;

    internal int part = 0;
    string email;
    string password;

    private static readonly string AuthKey = "AIzaSyDQmKtzV_j932WYvnxz4lNkViQTlHWFjEs";

    RealTimeDatabase database;

    // Start is called before the first frame update
    void Start()
    {
        database = RealTimeDatabase.instance;
        loadSence = LoadSence.instance;
    }

    // Update is called once per frame
    void Update()
    {
        switch (part)
        {
            case 0:
                loginForm.gameObject.SetActive(true);
                registerForm.gameObject.SetActive(false);
                loading.gameObject.SetActive(false);
                startMenu.gameObject.SetActive(false);
                ResetPasswordForm.gameObject.SetActive(false);
                break;
            case 1:
                loginForm.gameObject.SetActive(false);
                registerForm.gameObject.SetActive(true);
                loading.gameObject.SetActive(false);
                startMenu.gameObject.SetActive(false);
                ResetPasswordForm.gameObject.SetActive(false);
                break;
            case 2:
                loginForm.gameObject.SetActive(false);
                registerForm.gameObject.SetActive(false);
                loading.gameObject.SetActive(true);
                startMenu.gameObject.SetActive(false);
                ResetPasswordForm.gameObject.SetActive(false);
                break;
            case 3:
                loginForm.gameObject.SetActive(false);
                registerForm.gameObject.SetActive(false);
                loading.gameObject.SetActive(false);
                startMenu.gameObject.SetActive(true);
                ResetPasswordForm.gameObject.SetActive(false);
                break;
            case 4:
                loginForm.gameObject.SetActive(false);
                registerForm.gameObject.SetActive(false);
                loading.gameObject.SetActive(false);
                startMenu.gameObject.SetActive(false);
                ResetPasswordForm.gameObject.SetActive(true);
                break;
        }
    }

    public void login_Register_Button()
    {
        part = 1;
        ResetAllUIElement();
    }

    public void registetr_Back_Button()
    {
        part = 0;
        ResetAllUIElement();
    }

    public void login_Login_Button()
    {
        email = loginEmail.text;
        password = loginPassword.text;
        part = 2;
        if(email.Equals("")|| password.Equals(""))
        {
            part = 0;
            loginErrorText.text = "Error: That fields cannot be left blank";
        }
        else
        {
            //database.Login(email, password);

            StartCoroutine(loginUser(email, password));
        }
    }

    //Login User using HttpClient(option)
    IEnumerator loginUser(string email, string password)
    {
        HttpClient client = new HttpClient();
        var values = new Dictionary<string, string>
        {
            { "email", email },
            { "password", password },
            { "returnSecureToken", "true"}
        };

        var content = new FormUrlEncodedContent(values);

        var response = client.PostAsync("https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key=" + AuthKey, content);

        if (response.Result.IsSuccessStatusCode)
        {
            //Get Data
            string _response = response.Result.Content.ReadAsStringAsync().Result;
            SignResponse json = JsonUtility.FromJson<SignResponse>(_response);
            
            database.GetUserInfo(json.email, json.localId, json.idToken); //Save Data

            ResetAllUIElement(); //Reset Input Field

            UsernameSession.text = "Welcome: " + json.email;
            part = 3;  //Move to Menu page 
            Debug.Log("Success ");
        }
        else
        {
            loginPassword.text = "";
            loginErrorText.text = "Error: email or password incorrect";
            part = 0;
        }
        yield return null;
    }

    //Login User using HttpWebRequest(option) 
    IEnumerator LoginUser(string email, string password)
    {
        yield return null;
        string userData = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\",\"returnSecureToken\":true}";
       
        var request = (HttpWebRequest)WebRequest.Create(string.Format("https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key=" + AuthKey));

        var data = Encoding.ASCII.GetBytes(userData);

        request.Method = "POST";
        request.ContentType = "application/json";
        request.ContentLength = data.Length;

        using (var stream = request.GetRequestStream())
        {
            stream.Write(data, 0, data.Length);
        }

        try
        {
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            SignResponse sign = JsonUtility.FromJson<SignResponse>(responseString);
            if (sign.email != null)
            {
                database.GetUserInfo(sign.email, sign.localId, sign.idToken);
                ResetAllUIElement();
                UsernameSession.text = "Welcome: " + sign.email;
                part = 3;
                print("Success with LocalID: " + sign.localId);
            }
            else
            {
                part = 0;
                print("Fail");
            }
        }
        catch
        {
            loginErrorText.text = "Error: Email or Password incorrect!";
            part = 0;
            print("Fail with exception");
        }        
    }

    public void register_Register_Button()
    {
        email = registerEmail.text;
        password = registerPassword.text;
        part = 2;
        string confirmPassword = registerConfirmPassword.text;
        if (email.Equals("") || password.Equals("") || confirmPassword.Equals(""))
        {
            part = 1;
            registerErrorText.text = "Error: That fields cannot be left blank.";
        }
        else
        {
            if(password != confirmPassword)
            {
                part = 1;
                registerErrorText.text = "Error: Your confirm password incorrect.";
            }
            else
            {
                //database.Register(email, password);
                StartCoroutine(RegisterUser(email, password));
            }  
        }
    }

    IEnumerator RegisterUser(string email, string password)
    {
        yield return null;
        string userData = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\",\"returnSecureToken\":true}";

        var request = (HttpWebRequest)WebRequest.Create(string.Format("https://identitytoolkit.googleapis.com/v1/accounts:signUp?key=" + AuthKey));

        var data = Encoding.ASCII.GetBytes(userData);

        request.Method = "POST";
        request.ContentType = "application/json";
        request.ContentLength = data.Length;

        using (var stream = request.GetRequestStream())
        {
            stream.Write(data, 0, data.Length);
        }

        try
        {
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            SignResponse sign = JsonUtility.FromJson<SignResponse>(responseString);
            if (sign.email != null)
            {
                database.GetUserInfo(sign.email, sign.localId, sign.idToken);
                ResetAllUIElement();
                UsernameSession.text = "Welcome: " + sign.email;
                part = 3;
                print("Success with LocalID: " + sign.localId);
            }
            else
            {
                registerErrorText.text = "Error: This Account may be had existed or invalid. Please try again!";
                part = 1;
                print("Fail");
            }
        }
        catch
        {
            registerErrorText.text = "Error: This Account may be had existed or invalid. Please try again!";
            part = 1;
            print("Fail with exception");
        }
    }

    public void log_Out_Button()
    {
        part = 2;
        database.LogOut();
        part = 0;
        ResetAllUIElement();
    }

    public void Start_Button()
    {
        ResetAllUIElement();
        loadSence.LoadLevel(1);
    }

    public void ResetAllUIElement()
    {
        loginEmail.text = "";
        loginPassword.text = "";
        registerEmail.text = "";
        registerPassword.text = "";
        registerConfirmPassword.text = "";
        loginErrorText.text = "";
        registerErrorText.text = "";
        resetPassEmail.text = "";
        resetPassNotify.text = "";
        resetPassErrorText.text = "";
        email = null;
        password = null;
        print("clear");
    }

    public void login_ForgotPassword_Button()
    {
        part = 4;
        ResetAllUIElement();
    }

    public void resetPass_SendEmail_Butoon()
    {    
    }

    public void resetPass_Back()
    {
        part = 0;
        ResetAllUIElement();
    }

    public void GetError(string errorContent)
    {
        ErrorContent.text = errorContent;
        Errorform.gameObject.SetActive(true);
    }

    public void OK_Button()
    {
        Errorform.gameObject.SetActive(false);
    }
}
