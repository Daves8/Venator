using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml.Serialization;
using System.IO;

public class Quest1 : MonoBehaviour
{
    public bool QuestActive;
    public GameObject brother;
    public GameObject player;
    public Vector3 startPosition;
    public Text subtitles;
    public Text prompt;
    public Text target;
    public Cinemachine.CinemachineVirtualCamera groupCamera;
    public Cinemachine.CinemachineTargetGroup targetGroup;
    public Subquest subquest;

    private Fishing _riverFishing;
    private QuestsManagement _questManag;
    private Dialogue1 _dialogue1;

    private bool _startCoroutineSS = false;

    public enum Subquest
    {
        none = 0,
        subquest1 = 1, // Начало. Первый диалог с братом.
        subquest2, // Диалог окончен. Задача: выбрать из инвентаря удочку и начать рыбачить.
        subquest3, // Начата рыбалка. Задача: поймать рыбу.
        subquest4, // 1 рыба поймана. Задача: подойти к брату.
        subquest5, // Подошли к брату. Выбор: уходим или ловим дальше (уходим - карма на 0, словили 2 рыбы - карма +, словили 2+ рыбы - карма++).
        subquest6 // Рыбалка окончена. Задача: тдти с братом в деревню.
    }

    // Start is called before the first frame update
    void Start()
    {
        //brother.transform.position = startPosition;
        player.transform.position = startPosition;
        player.transform.rotation = Quaternion.Euler(0f, -175f, 0f);
        _riverFishing = GameObject.FindGameObjectWithTag("River").GetComponent<Fishing>();

        subtitles.text = "";
        prompt.text = "";

        subquest = Subquest.subquest1;

        groupCamera.enabled = false;
        targetGroup.m_Targets = new Cinemachine.CinemachineTargetGroup.Target[] { new Cinemachine.CinemachineTargetGroup.Target { target = player.transform, weight = 1f, radius = 0f }, new Cinemachine.CinemachineTargetGroup.Target { target = brother.transform, weight = 1f, radius = 0f } };

        _questManag = GetComponent<QuestsManagement>();
        _dialogue1 = Dialogue1.Load(_questManag.dialogues);
    }

    // Update is called once per frame
    void Update()
    {
        if (!QuestActive)
        {
            return;
        }

        switch (subquest)
        {
            case Subquest.subquest1:
                SubQ1();
                break;
            case Subquest.subquest2:
                SubQ2();
                break;
            case Subquest.subquest3:
                SubQ3();
                break;
            case Subquest.subquest4:
                SubQ4();
                break;
            case Subquest.subquest5:
                SubQ5();
                break;
            case Subquest.subquest6:
                SubQ6();
                break;
            default:
                groupCamera.enabled = false;
                break;
        }
        //enabled = false;
    }
    IEnumerator ShowSubtitles(string[] nodes)
    {
        _startCoroutineSS = true;
        foreach (string text in nodes)
        {
            subtitles.text = text;
            yield return new WaitForSeconds(3f);
        }
        subtitles.text = "";
        _startCoroutineSS = false;
        groupCamera.enabled = false;
        subquest = (Subquest)(int)++subquest;
    }
    private void SubQ1() // Начало. Первый диалог с братом.
    {
        target.text = "Поговорить с братом.";
        groupCamera.enabled = true;

        if (!_startCoroutineSS)
        {
            StartCoroutine(ShowSubtitles(_dialogue1.nodes[0].text));
        }
    }
    private void SubQ2() // Диалог окончен. Задача: выбрать из инвентаря удочку и начать рыбачить.
    {
        target.text = "Начать рыбачить.";
        prompt.text = "Чтобы начать рыбачить, зайдите в инвентарь (I) и нажмите на удочку.";

        if (_riverFishing.NowFishing)
        {
            subquest = Subquest.subquest3;
            prompt.text = "";
        }
    }
    private void SubQ3() // Начата рыбалка. Задача: поймать рыбу.
    {
        target.text = "Поймать рыбу.";
        prompt.text = "Чтобы поймать рыбу, быстро нажимайте E, когда увидете подсказку.";

        if (true/*проверка инвентаря на наличие рыбы*/)
        {
            subquest = Subquest.subquest4;
            prompt.text = "";
        }
    }
    private void SubQ4() // 1 рыба поймана. Задача: подойти к брату.
    {
        target.text = "Подойти к брату.";
        if (Vector3.Distance(player.transform.position, brother.transform.position) <= 1f)
        {
            subquest = Subquest.subquest5;
        }
    }
    private void SubQ5() // Подошли к брату. Выбор: уходим или ловим дальше (уходим - карма на 0, словили 2 рыбы - карма +, словили 2+ рыбы - карма++).
    {
        target.text = "Наловить еще рыбы.";
        groupCamera.enabled = true;

        if (!_startCoroutineSS)
        {
            StartCoroutine(ShowSubtitles(_dialogue1.nodes[1].text));
        }
        //subquest = Subquest.subquest6;
    }
    private void SubQ6() // Рыбалка окончена. Задача: тдти с братом в деревню.
    {
        target.text = "Пойти в деревню.";
        //subquest = Subquest.none;
    }
}

[XmlRoot("quest1")]
public class Dialogue1
{
    [XmlElement("node")]
    public Node[] nodes;

    public static Dialogue1 Load(TextAsset _xml)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Dialogue1));
        StringReader reader = new StringReader(_xml.text);
        Dialogue1 dial = serializer.Deserialize(reader) as Dialogue1;
        return dial;
    }
}

[System.Serializable]
public class Node
{
    [XmlElement("text")]
    public string[] text;
}