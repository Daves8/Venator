using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class QuestsManagement : MonoBehaviour
{
    public TextAsset dialoguesQ1;
    public TextAsset dialoguesQ2;
    public TextAsset dialoguesQ3;

    public Quest quest;
    private Quest _previousQuest;

    public GameObject button1;
    public GameObject button2;
    public GameObject button3;

    void Awake()
    {
        button1.SetActive(false);
        button2.SetActive(false);
        button3.SetActive(false);

        GetComponent<Quest1>().enabled = true;
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
                    GetComponent<Quest2>().enabled = false;
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

//[XmlRoot("quest1")]
//public class Dialogue1
//{
//    [XmlElement("node")]
//    public Node[] nodes;

//    public static Dialogue1 Load(TextAsset _xml)
//    {
//        XmlSerializer serializer = new XmlSerializer(typeof(Dialogue1));
//        StringReader reader = new StringReader(_xml.text);
//        Dialogue1 dial = serializer.Deserialize(reader) as Dialogue1;
//        return dial;
//    }
//}

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