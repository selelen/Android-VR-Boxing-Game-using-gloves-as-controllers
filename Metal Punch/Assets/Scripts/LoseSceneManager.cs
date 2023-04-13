
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseSceneManager : MonoBehaviour
{

    public static string sid;
    public static string tok;
    public static string cookies;
    public static string username;
    float _timeLeft=3.0f;
    // Start is called before the first frame update
    void Start()
    {

      sid= ManageApiConnections.getSid();
      tok=ManageApiConnections.getToken();
      cookies= ManageApiConnections.getCookies();
      username= ManageApiConnections.getUsername();
       ManageApiConnections.setVictory(0,cookies, username);
      
    }

    // Update is called once per frame
    void Update()
    {
          _timeLeft-= Time.deltaTime;
        if (_timeLeft<=0){
            
                SceneManager.LoadScene("MainMenuScr");
        }
    }
}
