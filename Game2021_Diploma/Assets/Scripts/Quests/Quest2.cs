using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class Quest2 : MonoBehaviour
{
    public GameObject hunter1;
    public GameObject hunter2;
    public GameObject hunter3;
    public GameObject hunter4;

    private GameObject _player;
    public Subquest subquest;

    private QuestsManagement _questManag;
    private Dialogue2 _dialogue2;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");

        _questManag = GetComponent<QuestsManagement>();
        _dialogue2 = Dialogue2.Load(_questManag.dialoguesQ2);
    }

    void Update()
    {
        Debug.Log("Квест 2!");

        switch (subquest)
        {
            case Subquest.subquest1:
                SubQ1();
                break;
            case Subquest.subquest2:
                //SubQ2();
                break;
            case Subquest.subquest3:
                //SubQ3();
                break;
            case Subquest.subquest4:
                //SubQ4();
                break;
            case Subquest.subquest5:
                //SubQ5();
                break;
            case Subquest.subquest6:
                //SubQ6();
                break;
            case Subquest.subquest7:
                //SubQ7();
                break;
            case Subquest.subquest8:
                //SubQ8();
                break;
            case Subquest.subquest9:
                //SubQ9();
                break;
            case Subquest.subquest10:
                //SubQ10();
                break;
            case Subquest.subquest11:
                //SubQ11();
                break;
            case Subquest.subquest12:
                //SubQ12();
                break;
            case Subquest.subquest13:
                //SubQ13();
                break;
            case Subquest.subquest14:
                //SubQ14();
                break;
            case Subquest.subquest15:
                //SubQ15();
                break;
            default:
                break;
        }
    }

    private void SubQ1()
    {

    }
}

[XmlRoot("quest2")]
public class Dialogue2
{
    [XmlElement("node")]
    public Node[] nodes;

    public static Dialogue2 Load(TextAsset _xml)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Dialogue2));
        StringReader reader = new StringReader(_xml.text);
        Dialogue2 dial = serializer.Deserialize(reader) as Dialogue2;
        return dial;
    }
}

public enum Subquest
{
    none = 0,
    subquest1 = 1, 
    subquest2,
    subquest3,
    subquest4,
    subquest5,
    subquest6,
    subquest7,
    subquest8,
    subquest9,
    subquest10,
    subquest11,
    subquest12,
    subquest13,
    subquest14,
    subquest15
}

//[System.Serializable]
//public class Node2
//{
//    [XmlElement("subtitles")]
//    public Subtitles2[] npcText;
//}

//[System.Serializable]
//public class Subtitles2
//{
//    [XmlAttribute("name")]
//    public string name;
//    [XmlElement("text")]
//    public string text;
//}