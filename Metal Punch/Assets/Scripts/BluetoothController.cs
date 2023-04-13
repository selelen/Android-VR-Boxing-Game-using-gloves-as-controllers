
using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System.Threading;

public class Sending : MonoBehaviour {

public static SerialPort sp = new SerialPort("COM3", 9600);
public string message2;
//float timePassed = 0.0f;
// Use this for initialization
void Start () {
sp.Open(); // opens the connection
sp.ReadTimeout = 16; // sets the timeout value before reporting error
print("Port Opened!");

}

// Update is called once per frame
void Update () {
//timePassed+=Time.deltaTime;
//if(timePassed>=0.2f){

//print("BytesToRead" +sp.BytesToRead);
message2 = sp.ReadLine();
print(message2);
// timePassed = 0.0f;
//}
}

void OnApplicationQuit()
{
sp.Close();
}

}