
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.IO.Ports;
using System;
using System.Threading;

public class ArduinoController : MonoBehaviour
{
   // SerialPortConnectorMac portConnect;
   public static SerialPort sp;
   public static string strIn;
   public static List<string> portList;
   public float smooth = 2.0F;
   private Vector3 prevPosition;
   public GameObject Arm;

   private float timer = 6f;

   private Rigidbody player;

   private Vector3 force;

   private int moveX = 0, moveY = 0;
   private int turnX = 0, turnY = 0;
   private float xAccl;   
   private float yAccl;
   private float zAccl;
    private float xGyro;
    private float yGyro;
    private float zGyro;

   private int axThreshold = 40; //move
   private int ayThreshold = 50; //unused
   private int gxThreshold = 35; //tilt move
   private int gyThreshold = 70; //launch ball 
   private int gyThresholdNeg = -30; //resetCamera/life

   public float moveAccScale = 200f;
   public float moveGyroScale = 500f;
   //public float powerScale = 10f;

   void MovementData(string s)
   {
     

       Debug.Log(s);
           string[] arduinoData = s.Split(';');
            Debug.Log(arduinoData);

           
               if(arduinoData[0]!="nan") xGyro = float.Parse(arduinoData[0])/100;   
               if(arduinoData[1]!="nan") yGyro = float.Parse(arduinoData[1])/100;
               if(arduinoData[2]!="nan") xAccl = float.Parse(arduinoData[2])/100;
               if(arduinoData[3]!="nan") yAccl = float.Parse(arduinoData[3])/100;
               if(arduinoData[4]!="nan")zAccl = float.Parse(arduinoData[4])/100;
              

                Debug.Log("xAccl:"+xAccl);
                Debug.Log("yAccl:"+yAccl);
                Debug.Log("zAccl:"+zAccl);
                Debug.Log("xGyro:"+xGyro);
                Debug.Log("yGyro:"+yGyro);
             
     
         
        Arm.transform.parent.localPosition=new Vector3 (xAccl,0.0f,zAccl+1);

   
   }


   void Start()
   {
       StopAllCoroutines(); //stop ReadFromArduino
       //StopCoroutine(AsynchronousReadFromArduino(null, null, 0f));
       //yield return null WaitForSeconds(1);
      // hand = (PlayerController)GameObject.Find("Player").GetComponent("PlayerController");

       try
       {
           Connect();
       }
       catch (Exception e)
       {
           Debug.Log(e.ToString() + " port reopening problem?");
       }
   }

   private void Connect()
   {
       string port = null;
       var pLength = System.IO.Ports.SerialPort.GetPortNames();
       //if (pLength.Length == 0) useController = false;

       foreach (string p in pLength)
       {
           //Debug.Log("p: " + p);
           port = p;
           Debug.Log(p);
       }
       sp = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);
       OpenConnection();
        Debug.Log("Connection openned");
       try
       {
           StartCoroutine(AsynchronousReadFromArduino((string s) => MovementData(s), () => Debug.LogError("err-sensor"), 10000f));
       }
       catch (Exception e)
       {
           Debug.Log(e.ToString() + " error starting coroutine");
       }
   }

   List<string> GetPortNames()
   {
       List<string> serialPorts = new List<string>();
       string[] ttys = Directory.GetFiles("/dev/", "tty.*");
       foreach (string dev in ttys)
       {
 
           serialPorts.Add(dev);
       }

       string[] cus = Directory.GetFiles("/dev/", "cu.*");
       foreach (string dev in cus)
       {

           serialPorts.Add(dev);
       }
       return serialPorts;
   }

   public void OpenConnection()
   {
       if (sp != null && !sp.IsOpen)
       {
           if (sp.IsOpen)
           {
               sp.Close();
               Debug.Log("Closing port, because it was already open!");
           }
           else
           {
               try
               {
                   sp.Open();
                   sp.ReadTimeout = 50;  // sets timeout value before reporting error
                   Debug.Log("Port Opened: " + sp.PortName);
                //    player.transform.localScale = player.transform.localScale * 1.5f;
               }
               catch (Exception e)
               {
                   Debug.Log(e.ToString() + " port reopening problem?");
               }
           }
       }
       else
       {
           if (sp.IsOpen) print("Port is already open");
           else print("Port == null");
       }
   }

   public IEnumerator AsynchronousReadFromArduino(Action<string> callback, Action fail = null, float timeout = float.PositiveInfinity)
   {
       DateTime initialTime = DateTime.Now;
       DateTime nowTime;
       TimeSpan diff = default(TimeSpan);
       string dataString = null;
       do
       {
           try
           {
               dataString = sp.ReadLine();
              
            
           }
           catch (TimeoutException)
           {
               dataString = null;
           }

           if (dataString != null)
           {
               try
               {
                   callback(obj: dataString);
               }
               catch (Exception e)
               {
                   Debug.Log(e.ToString());
                   dataString = null;
               }
               yield return null;
           }
           else yield return new WaitForSeconds(0.1f);

           nowTime = DateTime.Now;
           diff = nowTime - initialTime;

       } while (diff.Milliseconds < timeout);

       if (fail != null) fail();
       yield return null;
   }

   void OnApplicationQuit()
   {
     sp.Close();
   }

}
