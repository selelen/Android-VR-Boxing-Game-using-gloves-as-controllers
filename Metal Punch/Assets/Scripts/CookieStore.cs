using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookieStore : MonoBehaviour
{
    public string csrf;
    public string sid;

   
    public string returnSid(){
        return sid;
    }
    public string returnCsrf(){
        return csrf;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
