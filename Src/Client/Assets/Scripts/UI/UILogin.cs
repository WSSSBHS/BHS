using System.Collections;
using System.Collections.Generic;
using Services;
using UnityEngine;
using UnityEngine.UI;
using System.Data;

using System;

using SkillBridge.Message;
public class UILogin : MonoBehaviour
{
    
    public InputField username;
    public InputField password;
    public Button buttonLogin;



    private string connectionstring = "Server=127.0.0.1;Database=WSSSBHS\\MMORPG;User Id=sa;Password=LOSER;";
    // Start is called before the first frame update
    void Start()
    {
        
        UserService.Instance.OnLogin = OnLogin;
       
    }

  
    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClickLogin()
    {
       
        if (string.IsNullOrEmpty(this.username.text) )
        {

            MessageBox.Show("«Î ‰»Î’À∫≈");
            return;
        }

        if (string.IsNullOrEmpty(this.password.text))
        {
            MessageBox.Show("«Î ‰»Î√‹¬Î");
            return;
        }
        UserService.Instance.SendLogin(this.username.text, this.password.text);
    }

    void OnLogin(Result result, string msg)
    {
        if (result == Result.Success)
        {
            SceneManager.Instance.LoadScene("CharSelect");
        
        }
        else
        {
            MessageBox.Show("¥ÌŒÛ");

        }
    }
}
