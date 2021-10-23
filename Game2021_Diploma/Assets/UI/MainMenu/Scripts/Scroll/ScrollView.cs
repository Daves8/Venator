using System.Net;
using System.Xml.Linq;
using System.Runtime.Serialization;
using System.Security.AccessControl;
using System.Runtime.ExceptionServices;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ScrollView : MonoBehaviour
{   
    
    public Transform loadButtonParent;
    public GameObject loadButton;
    public GameObject scrollContent;

    private Transform btnTransform;
    private Transform scrollContentTransform;
    private RectTransform scrollContentRectTransform;
    private Vector3 scaleChange;
    private ItemButton itemButton;
    private static ArrayList buttons;
    

    void Awake()
    {
        btnTransform = loadButton.GetComponent<Transform>();
        scrollContentTransform = scrollContent.GetComponent<Transform>();
        scrollContentRectTransform = scrollContent.GetComponent<RectTransform>();
        buttons = new ArrayList();
    }

    public static bool isDirectoryContainFiles() 
    {
        string path = Application.persistentDataPath + "/Saves/";
        bool hasFiles=false;

        if(Directory.GetFiles(path).Length >0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static Dictionary<string, string> GetSaves()//Есть ли сохранения в папке да- возвращает коллекцию с путями , нет- возвращает пустую коллекцию
    {
        bool IsDirectoryContainFiles = ScrollView.isDirectoryContainFiles();
        Dictionary<string, string> saves = new Dictionary<string, string>();
        
        try
        {
            /*if (IsDirectoryContainFiles)
            {
                DirectoryInfo d = new DirectoryInfo(Application.persistentDataPath + "/Saves/");
                FileInfo[] Files = d.GetFiles("*.bin");

                foreach(FileInfo file in Files )
                {
                    Debug.Log("ПЕРЕД     "+ file.Name);
                    if(file.Name == "NewGame.bin") continue;
                    saves.Add(file.Name ,Application.persistentDataPath + "/Saves/" + file.Name);
                }
                
                return saves;
            }
            */
            
            
            if (IsDirectoryContainFiles)
            {   
                DirectoryInfo d = new DirectoryInfo(Application.persistentDataPath + "/Saves/");
                FileInfo[] Files = d.GetFiles("*.bin");
                int fileCount = Directory.GetFiles(Application.persistentDataPath + "/Saves/").Length;
                foreach(FileInfo file in Files )
                {
                    if(file.Name == "NewGame.bin")
                    fileCount -= 1;
                    
                }
                
                string[,] savesArray = new string[fileCount,2];
                
                int rows = 0;

                foreach(FileInfo file in Files )
                {
                    //Debug.Log("ПЕРЕД     "+ file.Name);
                    if(file.Name == "NewGame.bin") continue;                    
                    savesArray[rows,0] = file.Name;
                    savesArray[rows,1] = Application.persistentDataPath + "/Saves/" + file.Name;
                    rows++;  
                }

                
                //Debug.Log("File count " + fileCount);
                for (int i = fileCount - 1; i >= 0; i--)
                {
                    //Debug.Log("111 - " + savesArray[i,0] + "| 222 - "+ savesArray[i,1]);
                    saves.Add(savesArray[i,0],savesArray[i,1]);
                    
                }
                
                return saves;
            }     
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }

        return saves;
    }

    public void CreateButtons()//вызываем 1 раз в начале
    {   
        
        
        int y = 20;
        int contentHeight = 0;
        Dictionary<string, string> saves = ScrollView.GetSaves();//Создаем словарь с информацией о сохранениях
        
        try
        {
        foreach(GameObject item in buttons)
            {
            Destroy(item);
            }
        }
        catch(Exception e){}
        loadButton.SetActive(true);
        foreach (KeyValuePair<string, string> keyValue in saves)
        {  
            y-=20;
            contentHeight += 21;  
            
            GameObject obj = Instantiate(loadButton, loadButton.transform);
            buttons.Add(obj);
            obj.transform.SetParent(loadButtonParent, true);
            

            btnTransform = obj.GetComponent<Transform>();

            btnTransform.localPosition = new Vector3(0,y,0);            
            scrollContentRectTransform.sizeDelta = new Vector2(0, contentHeight);
            itemButton = obj.GetComponent<ItemButton>();
            
            itemButton.savePath = keyValue.Value;
            itemButton.mainButtonText.text = keyValue.Key;
            
            

            //Debug.Log("КЛЮЧ: " + keyValue.Key + " ЗНАЧЕНИЕ: " + keyValue.Value);
               
        }
        loadButton.SetActive(false);
        
    }

    public void DeleteSave(ItemButton saveButton)
    {
        File.Delete(saveButton.savePath);
        
        foreach(GameObject item in buttons)
        {
            Destroy(item);
        }
        CreateButtons();
    }
}