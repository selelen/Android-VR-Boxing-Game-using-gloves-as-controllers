
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerController : MonoBehaviour
{
    public GameObject LeftArm;
    public GameObject RightArm;

    public bool connected;

    public static void SetVictory(int victory){
        if(victory==1){
                SceneManager.LoadScene("WinScene");
        }
        else{
            SceneManager.LoadScene("LoseScene");
        }
    }

    private void Start() {
        BluetoothService.CreateBluetoothObject();
        connected= BluetoothService.StartBluetoothConnection("HC-06");
    }

    private void Update() {
 
        string data= BluetoothService.ReadFromBluetooth();
        Debug.Log("Data:"+data);
        
    }
 
}
