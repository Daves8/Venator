using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml.Serialization;
using System.IO;
using UnityEngine.AI;

public class Quest1 : MonoBehaviour
{
    public bool QuestActive;
    public GameObject brother;
    public GameObject player;
    public GameObject father;
    public GameObject mother;
    public GameObject home;
    public GameObject cart;
    public GameObject hunter1;
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

    private Button button1;
    private Button button2;
    private Button button3;

    private bool _startCoroutineSS = false;
    bool localCoroutQ5 = false;
    bool localCoroutQ8 = true;
    bool locCorQ10 = true;

    private int _idFish = 8;
    private int _resultQuest;
    private Player _scriptPlayer;

    private GameObject _targetDialogue;
    private NavMeshAgent _brNavMesh;

    private TargetPoint _targetPoint;

    public enum Subquest
    {
        none = 0,
        subquest1 = 1, // Начало. Первый диалог с братом.
        subquest2, // Диалог окончен. Задача: выбрать из инвентаря удочку и начать рыбачить.
        subquest3, // Начата рыбалка. Задача: поймать рыбу.
        subquest4, // 1 рыба поймана. Задача: подойти к брату.
        subquest5, // Подошли к брату. Выбор: уходим или ловим дальше (уходим - карма на 0, словили 2 рыбы - карма +, словили 2+ рыбы - карма++).
        subquest6, // Подходим к брату. Диалог с результатом выбора.
        subquest7, // Рыбалка окончена. Задача: тдти с братом в деревню.
        subquest8,
        subquest9,
        subquest10,
        subquest11,
        subquest12,
        subquest13,
        subquest14,
        subquest15
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
        //_questManag.quest = QuestsManagement.Quest.quest1;

        groupCamera.enabled = false;
        targetGroup.m_Targets = new Cinemachine.CinemachineTargetGroup.Target[] { new Cinemachine.CinemachineTargetGroup.Target { target = GameObject.FindGameObjectWithTag("HeadPlayer").transform, weight = 1f, radius = 0f }, new Cinemachine.CinemachineTargetGroup.Target { target = brother.transform.GetChild(0).transform, weight = 1f, radius = 0f } };
        _scriptPlayer = player.GetComponent<Player>();

        _targetDialogue = brother;
        _brNavMesh = brother.GetComponent<NavMeshAgent>();

        _targetPoint = GetComponent<TargetPoint>();

        _questManag = GetComponent<QuestsManagement>();
        _dialogue1 = Dialogue1.Load(_questManag.dialoguesQ1);
        button1 = _questManag.button1.GetComponent<Button>();
        button2 = _questManag.button2.GetComponent<Button>();
        button3 = _questManag.button3.GetComponent<Button>();
        button1.onClick.AddListener(But1);
        button2.onClick.AddListener(But2);
        button3.onClick.AddListener(But3);
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
            case Subquest.subquest7:
                SubQ7();
                break;
            case Subquest.subquest8:
                SubQ8();
                break;
            case Subquest.subquest9:
                SubQ9();
                break;
            case Subquest.subquest10:
                SubQ10();
                break;
            case Subquest.subquest11:
                SubQ11();
                break;
            case Subquest.subquest12:
                SubQ12();
                break;
            case Subquest.subquest13:
                SubQ13();
                break;
            case Subquest.subquest14:
                SubQ14();
                break;
            case Subquest.subquest15:
                SubQ15();
                break;
            default:
                groupCamera.enabled = false;
                break;
        }
        //enabled = false;
    }
    IEnumerator RotateToTarget()
    {
        while (_startCoroutineSS)
        {
            Vector3 dir = (_targetDialogue.transform.position - player.transform.position).normalized;
            Quaternion rot = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
            player.transform.rotation = Quaternion.Lerp(player.transform.rotation, rot, 5f * Time.deltaTime);

            Vector3 dir2 = (player.transform.position - _targetDialogue.transform.position).normalized;
            Quaternion rot2 = Quaternion.LookRotation(new Vector3(dir2.x, 0, dir2.z));
            _targetDialogue.transform.rotation = Quaternion.Lerp(_targetDialogue.transform.rotation, rot2, 5f * Time.deltaTime);
            yield return null;
        }
    }
    IEnumerator ShowSubtitles(Node nodes, bool nextQuest = true)
    {
        _startCoroutineSS = true;
        target.text = "";
        _targetPoint.target = null;
        CharacterMoving.IsReadyToMove = false;
        player.GetComponent<Battle>().AllowBattle = false;
        StartCoroutine(RotateToTarget());
        foreach (Subtitles subt in nodes.npcText)
        {
            if (subt.name == "Я")
            {
                player.GetComponent<Animator>().SetBool("Speak", true);
            }
            else
            {
                player.GetComponent<Animator>().SetBool("Speak", false);
            }
            subtitles.text = subt.name + ": ";
            subtitles.text += subt.text;
            yield return new WaitForSeconds(3f);
        }
        subtitles.text = "";
        _startCoroutineSS = false;
        groupCamera.enabled = false;
        CharacterMoving.IsReadyToMove = true;
        player.GetComponent<Battle>().AllowBattle = true;
        player.GetComponent<Animator>().SetBool("Speak", false);
        if (nextQuest)
        {
            subquest = (Subquest)(int)++subquest;
        }
    }
    private void SubQ1() // Начало. Первый диалог с братом.
    {
        _resultQuest = 0;
        if (!_startCoroutineSS)
        {
            target.text = "Поговорить с братом.";
            groupCamera.enabled = true;
            StartCoroutine(ShowSubtitles(_dialogue1.nodes[0]));
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

        if (_scriptPlayer.SearchInInventary(_idFish) >= 1)
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
        if (_resultQuest == 1)
        {
            return;
        }
        target.text = "Словить еще рыбы и подойти к брату";
        if (_scriptPlayer.SearchInInventary(_idFish) >= 2)
        {
            subquest = Subquest.subquest6;
            return;
        }
        if (_resultQuest == 2)
        {
            return;
        }

        if (localCoroutQ5)
        {
            CharacterMoving.IsReadyToMove = false;
            groupCamera.enabled = true;
            Cursor.lockState = CursorLockMode.None;
            button1.gameObject.SetActive(true);
            button1.gameObject.transform.GetChild(0).GetComponent<Text>().text = "Пойдём";
            button2.gameObject.SetActive(true);
            button2.gameObject.transform.GetChild(0).GetComponent<Text>().text = "Еще словлю";
            return;
        }

        if (!_startCoroutineSS)
        {
            CharacterMoving.IsReadyToMove = false;
            groupCamera.enabled = true;
            localCoroutQ5 = true;
            StartCoroutine(ShowSubtitles(_dialogue1.nodes[1], false));
        }

    }
    private void SubQ6() // Подходим к брату. Диалог с результатом выбора.
    {
        if (_resultQuest == 1)
        {
            subquest = Subquest.subquest7;
        }
        target.text = "Словить еще рыбы и подойти к брату";

        if (_scriptPlayer.SearchInInventary(_idFish) > 2)
        {
            _resultQuest = 3;
        }
        else if (_scriptPlayer.SearchInInventary(_idFish) == 2)
        {
            _resultQuest = 2;
        }

        if (Vector3.Distance(player.transform.position, brother.transform.position) <= 1f && _resultQuest >= 2 && !_startCoroutineSS)
        {
            groupCamera.enabled = true;
            StartCoroutine(ShowSubtitles(_dialogue1.nodes[_resultQuest + 2]));
        }
    }
    private void SubQ7() // Рыбалка окончена. Задача: идти с братом в деревню.
    {
        if (Vector3.Distance(player.transform.position, father.transform.position) < 10f)
        {
            target.text = "Подойти к отцу";

            _targetPoint.PointToTarget(father.transform);
        }
        else
        {
            target.text = "Идти за братом";

            _targetPoint.PointToTarget(brother.transform);
        }
        _brNavMesh.SetDestination(home.transform.position);

        if (Vector3.Distance(player.transform.position, father.transform.position) < 1.5f)
        {
            subquest = Subquest.subquest8;
        }
    }
    private void SubQ8() // Диалог с батей
    {
        if (localCoroutQ8)
        {
            targetGroup.m_Targets = new Cinemachine.CinemachineTargetGroup.Target[] { new Cinemachine.CinemachineTargetGroup.Target { target = GameObject.FindGameObjectWithTag("HeadPlayer").transform, weight = 1f, radius = 0f }, new Cinemachine.CinemachineTargetGroup.Target { target = father.transform.GetChild(0).transform, weight = 1f, radius = 0f } };
            localCoroutQ8 = false;
            groupCamera.enabled = true;
            _targetDialogue = father;
            StartCoroutine(ShowSubtitles(_dialogue1.nodes[6]));
        }
    }
    private void SubQ9()
    {
        target.text = "Зайти в дом и поговорить с матерью";
        _targetPoint.PointToTarget(mother.transform);
        if(Vector3.Distance(player.transform.position, mother.transform.position) < 1.5f)
        {
            //subquest = Subquest.subquest10;
            subquest = (Subquest)(int)++subquest;
        }
    }
    private void SubQ10() // Диалог с матерью
    {
        if (locCorQ10)
        {
            locCorQ10 = false;
            ChangeCompanion(mother);
            groupCamera.enabled = true;
            StartCoroutine(ShowSubtitles(_dialogue1.nodes[6 + _resultQuest]));
        }
    }
    private void SubQ11()
    {
        target.text = "Подойти к точке сбора";
        _targetPoint.PointToTarget(cart.transform);
        if (Vector3.Distance(player.transform.position, cart.transform.position) < 2f)
        {
            subquest = (Subquest)(int)++subquest;
        }
    }
    private void SubQ12()
    {
        if (!_startCoroutineSS)
        {
            ChangeCompanion(hunter1);
            groupCamera.enabled = true;
            StartCoroutine(ShowSubtitles(_dialogue1.nodes[10]));
            // ПОЛОЖИТЬ В ИНВЕНТАРЬ ИГРОКА ЛУК СО СТРЕЛАМИ
        }
    }
    private void SubQ13()
    {
        target.text = "Отправится на телеге в лес";
        _targetPoint.PointToTarget(cart.transform);

        if (true) // если игрок подошел к телеге, смотрит на нее и нажал на Е
        {
            // старт корутины (затухание экрана и перемещение игрока в точку)
            Invoke("NextQuest", 5f); // ПОМЕНЯТЬ ВРЕМЯ
        }
    }

    private void NextQuest()
    {
        _questManag.quest = QuestsManagement.Quest.quest2;
    }

    private void SubQ14()
    {

    }
    private void SubQ15()
    {

    }

    private void ChangeCompanion(GameObject companion)
    {
        targetGroup.m_Targets = new Cinemachine.CinemachineTargetGroup.Target[] { new Cinemachine.CinemachineTargetGroup.Target { target = GameObject.FindGameObjectWithTag("HeadPlayer").transform, weight = 1f, radius = 0f }, new Cinemachine.CinemachineTargetGroup.Target { target = companion.transform.GetChild(0).transform, weight = 1f, radius = 0f } };
        _targetDialogue = companion;
    }

    private void But1()
    {
        StartCoroutine(ShowSubtitles(_dialogue1.nodes[2]));
        _resultQuest = 1;
        localCoroutQ5 = false;
        button1.gameObject.SetActive(false);
        button2.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void But2()
    {
        StartCoroutine(ShowSubtitles(_dialogue1.nodes[3], false));
        _resultQuest = 2;
        localCoroutQ5 = false;
        button1.gameObject.SetActive(false);
        button2.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void But3()
    {

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

//[System.Serializable]
//public class Node
//{
//    [XmlElement("subtitles")]
//    public Subtitles[] npcText;
//}

//[System.Serializable]
//public class Subtitles
//{
//    [XmlAttribute("name")]
//    public string name;
//    [XmlElement("text")]
//    public string text;
//}