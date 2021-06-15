using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Quest2 : MonoBehaviour
{
    public GameObject hunter1;
    public GameObject hunter2;
    public GameObject hunter3;
    public GameObject hunter4;

    private GameObject _player;
    private Player _scriptPlayer;
    private PlayerCharacteristics _playerCharact;

    private QuestsManagement _questManag;
    private Dialogue2 _dialogue2;

    private TextMeshProUGUI _subtitles;
    private TextMeshProUGUI _prompt;
    private TextMeshProUGUI _target;
    private Button button1;
    private Button button2;
    private Button button3;
    private Cinemachine.CinemachineVirtualCamera _groupCamera;
    private Cinemachine.CinemachineTargetGroup _targetGroup;
    private GameObject _targetDialogue;
    private TargetPoint _targetPoint;

    private Transform _glade;
    private ForestAnimal _boar;

    private GameObject _mother;
    private GameObject _father;
    private GameObject _innkeeper;


    private bool _coroutSS;
    private bool _startCoroutineSC;
    private int _resultButtn;

    // сохранять
    public Subquest subquest;
    public int resultQuest;

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

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _scriptPlayer = _player.GetComponent<Player>();
        _glade = GameObject.FindGameObjectWithTag("Glade").transform;
        _questManag = GetComponent<QuestsManagement>();
        _dialogue2 = Dialogue2.Load(_questManag.dialoguesQ2);
        _targetDialogue = hunter1;
        _groupCamera.enabled = false;
        _targetGroup.m_Targets = new Cinemachine.CinemachineTargetGroup.Target[] { new Cinemachine.CinemachineTargetGroup.Target { target = GameObject.FindGameObjectWithTag("HeadPlayer").transform, weight = 1f, radius = 0f }, new Cinemachine.CinemachineTargetGroup.Target { target = _targetDialogue.transform.GetChild(0).transform, weight = 1f, radius = 0f } };
        subquest = Subquest.subquest1; // сзапписываем
        resultQuest = 0; //
        button1 = _questManag.button1.GetComponent<Button>();
        button2 = _questManag.button2.GetComponent<Button>();
        button3 = _questManag.button3.GetComponent<Button>();
        _subtitles = _questManag.subtitles.GetComponent<TextMeshProUGUI>();
        _prompt = _questManag.prompt.GetComponent<TextMeshProUGUI>();
        _target = _questManag.target.GetComponent<TextMeshProUGUI>();
        button1.onClick.AddListener(But1);
        button2.onClick.AddListener(But2);
        button3.onClick.AddListener(But3);
        _subtitles.text = "";
        _prompt.text = "";
        _target.text = "";
        _targetPoint = GetComponent<TargetPoint>();
        _boar = GameObject.FindGameObjectsWithTag("Boar")[0].GetComponent<ForestAnimal>();
        _startCoroutineSC = false;
        _coroutSS = true;
        _resultButtn = 0;
        _playerCharact = _player.GetComponent<PlayerCharacteristics>();
        _mother = _questManag.mother;
        _father = _questManag.father;
        _innkeeper = _questManag.innkeeper;
    }

    void Update()
    {
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
                break;
        }
    }

    private IEnumerator ShowSubtitles(int node)
    {
        _coroutSS = false;
        Node nodes = _dialogue2.nodes[node];
        foreach (Subtitles subt in nodes.npcText)
        {
            _subtitles.text = subt.name + ": ";
            _subtitles.text += subt.text;
            yield return new WaitForSeconds(3f);
        }
        _subtitles.text = "";
    }
    IEnumerator RotateToTarget()
    {
        while (_startCoroutineSC)
        {
            Vector3 dir = (_targetDialogue.transform.position - _player.transform.position).normalized;
            Quaternion rot = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
            _player.transform.rotation = Quaternion.Lerp(_player.transform.rotation, rot, 5f * Time.deltaTime);

            Vector3 dir2 = (_player.transform.position - _targetDialogue.transform.position).normalized;
            Quaternion rot2 = Quaternion.LookRotation(new Vector3(dir2.x, 0, dir2.z));
            _targetDialogue.transform.rotation = Quaternion.Lerp(_targetDialogue.transform.rotation, rot2, 5f * Time.deltaTime);
            yield return null;
        }
    }
    IEnumerator ShowCutscene(int node, bool nextQuest = true)
    {
        Node nodes = _dialogue2.nodes[node];
        _startCoroutineSC = true;
        _target.text = "";
        _targetPoint.target = null;
        _groupCamera.enabled = true;
        CharacterMoving.IsReadyToMove = false;
        _player.GetComponent<Battle>().AllowBattle = false;
        StartCoroutine(RotateToTarget());
        foreach (Subtitles subt in nodes.npcText)
        {
            _subtitles.text = subt.name + ": ";
            _subtitles.text += subt.text;
            yield return new WaitForSeconds(3f);
        }
        _subtitles.text = "";
        _target.text = "";
        _startCoroutineSC = false;
        _groupCamera.enabled = false;
        CharacterMoving.IsReadyToMove = true;
        _player.GetComponent<Battle>().AllowBattle = true;
        //_player.GetComponent<Animator>().SetBool("Speak", false);
        if (nextQuest)
        {
            subquest = (Subquest)(int)++subquest;
        }
    }
    private void ChangeCompanion(GameObject companion)
    {
        _targetGroup.m_Targets = new Cinemachine.CinemachineTargetGroup.Target[] { new Cinemachine.CinemachineTargetGroup.Target { target = GameObject.FindGameObjectWithTag("HeadPlayer").transform, weight = 1f, radius = 0f }, new Cinemachine.CinemachineTargetGroup.Target { target = companion.transform.GetChild(0).transform, weight = 1f, radius = 0f } };
        _targetDialogue = companion;
    }

    #region Задания
    private void SubQ1()
    {
        // игрок в лесу, цель: выследить кабана, а именно подойти к поляне (тогда там спавнится кабан??)
        _target.text = "Идти к поляне";
        _targetPoint.PointToTarget(_glade.transform);
        if (_coroutSS)
        {
            StartCoroutine(ShowSubtitles(0));
        }
        if (Vector3.Distance(_player.transform.position, _glade.position) <= 5.0f)
        {
            subquest = (Subquest)(int)++subquest;
            _coroutSS = true;
        }

        foreach (var item in new GameObject[] { hunter1, hunter2, hunter3, hunter4 })
        {
            item.GetComponent<Hunter>().WalkTo(_glade.transform);
        }

    }
    private void SubQ2()
    {
        // увидели? кабана, цель: убить кабана, подсказка: как стрелять из лука
        _target.text = "Убить кабана";
        _prompt.text = "Для выстрела из лука зажмите кнопку выстрела, пока стрела не зарядится, и отпуатите";
        _targetPoint.PointToTarget(_boar.gameObject.transform);
        foreach (var item in new GameObject[] { hunter1, hunter2, hunter3, hunter4 })
        {
            item.GetComponent<Hunter>().Agressive(true);
        }
        if (_coroutSS)
        {
            StartCoroutine(ShowSubtitles(1));
        }
        if (_boar._die)
        {
            subquest = (Subquest)(int)++subquest;
            _coroutSS = true;
        }
    }
    private void SubQ3()
    {
        // освежевать его
        _target.text = "Освежевать кабана";
        _targetPoint.PointToTarget(_boar.gameObject.transform);
        foreach (var item in new GameObject[] { hunter1, hunter2, hunter3, hunter4 })
        {
            item.GetComponent<Hunter>().Agressive(false);
        }
        if (_coroutSS)
        {
            StartCoroutine(ShowSubtitles(2));
        }
        if (_scriptPlayer.SearchInInventary(6) >= 1) // в инвентаре есть шкура и мясо кабана? 0 - шкура, 1- мясо
        {
            subquest = (Subquest)(int)++subquest;
            _coroutSS = true;
        }
    }
    private void SubQ4()
    {
        // подойти к охотникам (выбор еще охотимся или идем домой)
        _target.text = "Подойти к охотникам";
        _targetPoint.PointToTarget(hunter1.transform);
        if (!_startCoroutineSC && Vector3.Distance(_player.transform.position, hunter1.transform.position) <= 1.5f)
        {
            StartCoroutine(ShowCutscene(3));
        }
    }
    private void SubQ5()
    {
        button1.gameObject.SetActive(true);
        button2.gameObject.SetActive(true);

        _target.text = "";
        _targetPoint.target = null;
        _groupCamera.enabled = true;
        CharacterMoving.IsReadyToMove = false;
        _player.GetComponent<Battle>().AllowBattle = false;
        StartCoroutine(RotateToTarget());
    }
    private void SubQ6() // уходим, больше не охотимся
    {
        if (!_startCoroutineSC)
        {
            StartCoroutine(ShowCutscene(4));
            foreach (var item in new GameObject[] { hunter1, hunter2, hunter3, hunter4 })
            {
                item.GetComponent<Hunter>().WalkTo(GameObject.FindGameObjectWithTag("ForestCart").transform);
            }
        }
    }
    private void SubQ7()
    {
        _target.text = "Отправиться в деревню";
        _targetPoint.PointToTarget(GameObject.FindGameObjectWithTag("ForestCart").transform);
        if (_playerCharact.place == PlayerCharacteristics.Place.village)
        {
            subquest = (Subquest)13;// совмещаем ветки квеста
        }
    }
    private void SubQ8() // остаемся, еще на одного нападаем 
    {
        if (!_startCoroutineSC)
        {
            StartCoroutine(ShowCutscene(5));
            foreach (var item in new GameObject[] { hunter1, hunter2, hunter3, hunter4 })
            {
                item.GetComponent<Hunter>().WalkTo(GameObject.FindGameObjectWithTag("ForestCart").transform);
            }
        }
    }
    private void SubQ9()
    {
        _target.text = "Добыть и освежевать кабана";
        _targetPoint.PointToTarget(null);
        if (_scriptPlayer.inventory.FindItemOnInventory(6) > 1)
        {
            subquest = (Subquest)(int)++subquest;
        }
    }
    private void SubQ10()
    {
        _target.text = "Вернуться к охотникам";
        _targetPoint.PointToTarget(hunter1.transform);
        if (Vector3.Distance(_player.transform.position, hunter1.transform.position) < 2f)
        {
            subquest = (Subquest)(int)++subquest;
        }
    }
    private void SubQ11()
    {
        if (!_startCoroutineSC)
        {
            if (_scriptPlayer.inventory.FindItemOnInventory(6) == 2)
            {
                StartCoroutine(ShowCutscene(6));
            }
            else if (_scriptPlayer.inventory.FindItemOnInventory(6) > 2)
            {
                StartCoroutine(ShowCutscene(7));
            }
        }
    }
    private void SubQ12()
    {
        _target.text = "Отправиться в деревню";
        _targetPoint.PointToTarget(GameObject.FindGameObjectWithTag("ForestCart").transform);
        if (_playerCharact.place == PlayerCharacteristics.Place.village)
        {
            foreach (var item in new GameObject[] { hunter1, hunter2, hunter3, hunter4 })
            {
                Transform tr = new GameObject().transform;
                tr.position = new Vector3(946f, 8f, 767f);
                item.GetComponent<Hunter>().TeleportTo(tr);
            }
            ChangeCompanion(_mother);
            subquest = (Subquest)13;
        }
    }
    private void SubQ13()
    {
        // мы в деревне. задача: подойти к матери, дать одну шкуру, она пошлет к корчмарю и ему продать все
        _target.text = "Подойти к матери";
        _targetPoint.PointToTarget(_mother.transform);
        if (!_startCoroutineSC && Vector3.Distance(_player.transform.position, _mother.transform.position) < 2f)
        {
            if (_scriptPlayer.inventory.FindItemOnInventory(6) == 1)
            {
                StartCoroutine(ShowCutscene(8));
            }
            else if (_scriptPlayer.inventory.FindItemOnInventory(6) == 2)
            {
                StartCoroutine(ShowCutscene(9));
            }
            else if (_scriptPlayer.inventory.FindItemOnInventory(6) > 2)
            {
                StartCoroutine(ShowCutscene(10));
            }
        }
    }
    private void SubQ14()
    {
        _target.text = "Подойти к трактирщику";
        _targetPoint.PointToTarget(_innkeeper.transform);
        ChangeCompanion(_innkeeper);
        if (Vector3.Distance(_player.transform.position, _innkeeper.transform.position) < 2f)
        {
            if (_scriptPlayer.inventory.FindItemOnInventory(6) == 1)
            {
                resultQuest = 1;
            }
            else if (_scriptPlayer.inventory.FindItemOnInventory(6) == 2)
            {
                resultQuest = 2;
            }
            else if (_scriptPlayer.inventory.FindItemOnInventory(6) > 2)
            {
                resultQuest = 3;
            }
            _questManag.resultQuests[1] = resultQuest;
            _questManag.quest = QuestsManagement.Quest.quest3;
        }
    }
    private void SubQ15()
    {

    }
    #endregion

    private void But1()
    {
        _resultButtn = 1;
        button1.gameObject.SetActive(false);
        button2.gameObject.SetActive(false);
        subquest = (Subquest)6;
    }
    private void But2()
    {
        _resultButtn = 2;
        button1.gameObject.SetActive(false);
        button2.gameObject.SetActive(false);
        subquest = (Subquest)8;
    }
    private void But3()
    {
        _resultButtn = 3;
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