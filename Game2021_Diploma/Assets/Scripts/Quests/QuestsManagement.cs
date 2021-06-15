﻿using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;

public class QuestsManagement : MonoBehaviour
{
    public TextAsset dialoguesQ1;
    public TextAsset dialoguesQ2;
    public TextAsset dialoguesQ3;

    public Quest quest;
    private Quest _previousQuest;
    public int[] resultQuests;

    public GameObject button1;
    public GameObject button2;
    public GameObject button3;

    public TextMeshProUGUI subtitles;
    public TextMeshProUGUI prompt;
    public TextMeshProUGUI target;

    public GameObject mother;
    public GameObject father;
    public GameObject innkeeper;

    void Awake()
    {
        button1.SetActive(false);
        button2.SetActive(false);
        button3.SetActive(false);

        GetComponent<Quest1>().enabled = false;
        GetComponent<Quest2>().enabled = false;
        GetComponent<Quest3>().enabled = false;
        //GetComponent<Quest4>().enabled = false;

        quest = Quest.quest1; // ПОМЕНЯТЬ НА КВЕСТ 1--------------------------------------------------------------------------------------
        resultQuests = new int[4];
    }

    void Update()
    {
        if (_previousQuest != quest)
        {
            switch (quest)
            {
                case Quest.quest1:
                    GetComponent<Quest1>().enabled = true;
                    GetComponent<Quest2>().enabled = false;
                    GetComponent<Quest3>().enabled = false;
                    //GetComponent<Quest4>().enabled = false;
                    break;
                case Quest.quest2:
                    GetComponent<Quest1>().enabled = false;
                    GetComponent<Quest2>().enabled = true;
                    GetComponent<Quest3>().enabled = false;
                    //GetComponent<Quest4>().enabled = false;
                    break;
                case Quest.quest3:
                    GetComponent<Quest1>().enabled = false;
                    GetComponent<Quest2>().enabled = false;
                    GetComponent<Quest3>().enabled = true;
                    //GetComponent<Quest4>().enabled = false;
                    break;
                case Quest.quest4:
                    GetComponent<Quest1>().enabled = false;
                    GetComponent<Quest2>().enabled = false;
                    GetComponent<Quest3>().enabled = false;
                    //GetComponent<Quest4>().enabled = true;
                    break;
                default:
                    break;
            }
        }
        _previousQuest = quest;
    }

    public enum Quest
    {
        none = 0,
        quest1 = 1,
        quest2,
        quest3,
        quest4
    }
}

[System.Serializable]
public class Node
{
    [XmlElement("subtitles")]
    public Subtitles[] npcText;
}

[System.Serializable]
public class Subtitles
{
    [XmlAttribute("name")]
    public string name;
    [XmlElement("text")]
    public string text;
}