
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultHandler : MonoBehaviour
{
   static int difficultLevel=1;
    
    public static int getDifficultLevel(){
        return difficultLevel;
    }

    public void setDifficultLevel(int level){
        difficultLevel= level;
        Debug.Log("Currently at level "+level);
        SceneManager.LoadScene("GameScr");
    }

    
}
