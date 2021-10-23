using System.Net.Security;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Net.Mime;
using System.Linq.Expressions;
using System;
using System.Security.AccessControl;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem 
{
    

    public static void SavePlayer (Player player)//DataExists
    {
        BinaryFormatter formatter = new BinaryFormatter();

        if(!Directory.Exists(Application.persistentDataPath + "/Saves"))
        {
            System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/Saves");
        }

        string path = Application.persistentDataPath + "/Saves/"+DateTime.Now.ToString("yyyy M dd  HH mm ss")+".bin";//1
        //Debug.Log(path);
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player);

        formatter.Serialize(stream, data);        
        stream.Close();

    }

    public static void SavePlayer ()//NullData
    {
        BinaryFormatter formatter = new BinaryFormatter();

        if(!Directory.Exists(Application.persistentDataPath + "/Saves"))
        {
            System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/Saves");
        }

        string path = Application.persistentDataPath + "/Saves/"+ "NewGame" +".bin";//1
        Debug.Log(path);
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData();

        formatter.Serialize(stream, data);        
        stream.Close();

    }

    public static PlayerData LoadPlayer(string path)
    {
        
        
        //string path = Application.persistentDataPath + "/Saves/"+DateTime.Now+".bin";// НЕЗАБУДЬ!!!!!!!!!!
        
        if(File.Exists(path))
        {
            //Debug.Log(path);
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data; 
        }else
        {
            //Debug.Log("Save file not found in:" + path);
            return null;
        }
    }
    
}
