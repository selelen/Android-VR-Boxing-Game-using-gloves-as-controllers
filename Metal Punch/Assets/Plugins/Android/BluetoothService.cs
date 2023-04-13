using System;
using UnityEngine;


public class BluetoothService
{

    private static AndroidJavaClass unityPlayer;
    private static AndroidJavaObject activity;
    private static AndroidJavaObject context;
    private static AndroidJavaClass unity3dbluetoothplugin;
    private static AndroidJavaObject BluetoothConnector;


    // creating an instance of the bluetooth class from the plugin 
    public static void CreateBluetoothObject()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            context = activity.Call<AndroidJavaObject>("getApplicationContext");
            unity3dbluetoothplugin = new AndroidJavaClass("com.example.unity3dbluetoothplugin.BluetoothConnector");
            BluetoothConnector = unity3dbluetoothplugin.CallStatic<AndroidJavaObject>("getInstance");
        }
    }

    // starting bluetooth connection with device named "DeviceName"
    // print the status on the screen using native android Toast
    public static bool StartBluetoothConnection(string DeviceName)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            string connectionStatus = "non";
            try
            {
                connectionStatus = BluetoothConnector.Call<string>("StartBluetoothConnection", DeviceName);
                BluetoothConnector.Call("PrintOnScreen", context, connectionStatus);
                if (connectionStatus == "Connected") return true;
                else return false;


            }
            catch (Exception e)
            {
                BluetoothConnector.Call("PrintOnScreen", context, connectionStatus);
                return false;
            }
        }
        else return false;
    }

    // should be called inside OnApplicationQuit
    // stop connection with the bluetooth device
    public static void StopBluetoothConnection()
    {

        if (Application.platform == RuntimePlatform.Android)
        {
            try
            {
                BluetoothConnector.Call("StopBluetoothConnection");
                BluetoothConnector.Call("PrintOnScreen", context, "connction stoped");

            }
            catch (Exception e)
            {
                BluetoothConnector.Call("PrintOnScreen", context, "stop connction error");
            }
        }
    }

    // write data as a string to the bluetooth device
    public static void WritetoBluetooth(string data)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            try
            {
                BluetoothConnector.Call("WriteData", data);


            }
            catch (Exception e)
            {
                BluetoothConnector.Call("PrintOnScreen", context, "write data error");
            }
        }
    }


    //read data from the bluetooth device
    // if there is an error or there is no data coming, this method will return "" as an output
    public static string ReadFromBluetooth()
    {
        string data = "";
        if (Application.platform == RuntimePlatform.Android)
        {
            try
            {
                data = BluetoothConnector.Call<string>("ReadData");
                return data;
            }
            catch (Exception e)
            {
                BluetoothConnector.Call("PrintOnScreen", context, "read data error");
                return "";
            }
        }
        else return "";

    }
}
