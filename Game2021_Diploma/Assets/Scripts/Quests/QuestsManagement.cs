using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestsManagement : MonoBehaviour
{
    public TextAsset dialogues;

    public Quest quest;
    private Quest _previousQuest;

    public GameObject button1;
    public GameObject button2;
    public GameObject button3;

    void Start()
    {
        button1.SetActive(false);
        button2.SetActive(false);
        button3.SetActive(false);

        GetComponent<Quest1>().enabled = false;
        GetComponent<Quest2>().enabled = false;
    }

    void Update()
    {
        if (_previousQuest != quest)
        {
            switch (quest)
            {
                case Quest.quest1:
                    GetComponent<Quest1>().enabled = true;
                    break;
                case Quest.quest2:
                    GetComponent<Quest1>().enabled = false;
                    GetComponent<Quest2>().enabled = true;
                    break;
                case Quest.quest3:
                    //GetComponent<Quest2>().enabled = false;
                    //GetComponent<Quest3>().enabled = true;
                    break;
                default:
                    break;
            }
        }
        _previousQuest = quest;
    }

    public enum Quest
    {
        quest1 = 1,
        quest2,
        quest3
    }
}
