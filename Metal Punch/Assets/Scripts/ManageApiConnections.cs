
using System.Net.Http.Headers;
using System.Drawing;
using System.Net.Mime;
using System;
using System.Net;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;


public class ManageApiConnections : MonoBehaviour{

    public InputField username;
    public InputField userPassword;
    public static string cookies;
    public static string session_id;
    public static string csrf;
    public static string user;
    public void loginUser(){
        Debug.Log(username.text);
        Debug.Log(userPassword.text);
         HttpWebRequest request= (HttpWebRequest)WebRequest.Create("http://20.70.7.244:8000/players/login/"+username.text+"/"+userPassword.text);
         HttpWebResponse response = (HttpWebResponse)request.GetResponse();
          StreamReader reader= new StreamReader(response.GetResponseStream());
        string json = reader.ReadToEnd();
        Debug.Log(json);
        if(json!="Invalid login"){
               
                var Headers=response.Headers;
                var cookie = response.GetResponseHeader("Set-Cookie");
                setCookies(cookie);
                Debug.Log(cookie);
                    Regex rxCookie = new Regex("csrftoken=(?<csrf_token>.{64});");
                 MatchCollection cookieMatches = rxCookie.Matches (cookie);
                    string csrfCookie = cookieMatches[0].Groups ["csrf_token"].Value;
                    Regex idCookie = new Regex("sessionid=(?<session_id>.{32});");
                     MatchCollection idcookieMatches = idCookie.Matches (cookie);
                    string sessionid = idcookieMatches[0].Groups ["session_id"].Value;
                Debug.Log("CRSF:"+csrfCookie);
                Debug.Log("ID:"+sessionid);

                setSid(sessionid);
                setToken(csrfCookie);
                setUsername(username.text);
                SceneManager.LoadScene("MainMenuScr");
        }else{
            Text textUser = username.transform.Find("Text").GetComponent<Text>();
            textUser.color = Color.red;
            Text textPass = userPassword.transform.Find("Text").GetComponent<Text>();
            textPass.color=Color.red;
        }
       
    }
    public static void registerUser(){
        Application.OpenURL("http://20.70.7.244:3000");
    }
    public static void logoutUser(){
         SceneManager.LoadScene("LoginMenu");
        HttpWebRequest request= (HttpWebRequest)WebRequest.Create("http://20.70.7.244:8000/players/logout");
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
       

    }
    public static Player getUserData(string username){
        HttpWebRequest request= (HttpWebRequest)WebRequest.Create("http://20.70.7.244:8000/players/api/players?username="+username);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader= new StreamReader(response.GetResponseStream());
        string json = reader.ReadToEnd();
        return JsonUtility.FromJson<Player>(json);
    }
    public static void getRanking(){
        Application.OpenURL("http://20.70.7.244:3000");
    }

    public void setSid(string sid){
        session_id=sid;
    }
    public void setUsername(string username){
        user=username;
    }
    public void setToken(string tok){
        csrf=tok;
    }
    public static string getToken(){
        return csrf;
    }
    public static string getSid(){
        return session_id;
    }
    public void setCookies(string cookie){
        cookies=cookie;
    }
    public static string getCookies(){
        return cookies;
    }
    public static string getUsername(){
        return user;
    }
    public static void setVictory(int victory,string cookie, string username){
        Debug.Log("Set victory"+victory);
      var level = DifficultHandler.getDifficultLevel();
        Debug.Log("Difficult:"+level);

        if(victory==1){
    HttpWebRequest request= (HttpWebRequest)WebRequest.Create("http://20.70.7.244:8000/players/score/"+username+"/win/"+level);

   request.Method="GET";
   request.Headers["Cookie"]=cookie;

              
    request.Accept="application/vnd.onem2m-res+json;";
    request.Headers["Accept-encoding"]="gzip, deflate";
    request.Headers["Accept-Language"]="es-ES,es;q=0.9";
    request.Headers["Cache-Control"]="max-age=0";
    request.UserAgent="Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/101.0.0.0 Mobile Safari/537.36";
    Debug.Log(request.Headers);
    try{
    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
    }catch (WebException e)
    {
   string pageContent = new StreamReader(e.Response.GetResponseStream()).ReadToEnd().ToString();
   Debug.Log(pageContent);
    }
        }
        else{
       HttpWebRequest request= (HttpWebRequest)WebRequest.Create("http://20.70.7.244:8000/players/score/"+username+"/lose/"+level);

   request.Method="GET";
   request.Headers["Cookie"]=cookie;

              
    request.Accept="application/vnd.onem2m-res+json;";
    request.Headers["Accept-encoding"]="gzip, deflate";
    request.Headers["Accept-Language"]="es-ES,es;q=0.9";
    request.Headers["Cache-Control"]="max-age=0";
    request.UserAgent="Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/101.0.0.0 Mobile Safari/537.36";
    Debug.Log(request.Headers);
    try{
    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
    }catch (WebException e)
    {
   string pageContent = new StreamReader(e.Response.GetResponseStream()).ReadToEnd().ToString();
   Debug.Log(pageContent);
    }
        }
    }
}
