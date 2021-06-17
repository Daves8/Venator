using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Quest4 : MonoBehaviour
{
    private GameObject _player;
    private Player _scriptPlayer;
    private PlayerCharacteristics _playerCharact;

    private QuestsManagement _questManag;
    private Dialogue4 _dialogue4;

    private GameObject[] allySoldiers;
    private GameObject[] partisans;
    private GameObject[] enemySoldiers;

    private TextMeshProUGUI _subtitles;
    private TextMeshProUGUI _prompt;
    private TextMeshProUGUI _target;
    private Button button1;
    private Button button2;
    private Button button3;
    public Cinemachine.CinemachineVirtualCamera _groupCamera;
    public Cinemachine.CinemachineTargetGroup _targetGroup;
    private GameObject _targetDialogue;
    private TargetPoint _targetPoint;
    private Transform _entranceToTavern;
    private Transform tr;

    private bool _coroutSS;
    private bool _startCoroutineSC;
    private int _resultButtn;
    private bool _helpTel;
    private bool _helpQue;
    private bool _help3;
    private bool _help6;

    //public GameObject[] titles;

    // сохранять
    public Subquest subquest;

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
    }

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _scriptPlayer = _player.GetComponent<Player>();
        _questManag = GetComponent<QuestsManagement>();
        _dialogue4 = Dialogue4.Load(_questManag.dialoguesQ4);
        allySoldiers = GameObject.FindGameObjectsWithTag("Enemy");
        partisans = GameObject.FindGameObjectsWithTag("Hunter");
        enemySoldiers = GameObject.FindGameObjectsWithTag("EnemySoldier");
        _groupCamera.enabled = false;
        _targetDialogue = _questManag.father;
        _targetGroup.m_Targets = new Cinemachine.CinemachineTargetGroup.Target[] { new Cinemachine.CinemachineTargetGroup.Target { target = GameObject.FindGameObjectWithTag("HeadPlayer").transform, weight = 1f, radius = 0f }, new Cinemachine.CinemachineTargetGroup.Target { target = _targetDialogue.transform.GetChild(0).transform, weight = 1f, radius = 0f } };

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
        _startCoroutineSC = false;
        _coroutSS = true;
        _resultButtn = 0;
        _playerCharact = _player.GetComponent<PlayerCharacteristics>();

        CharacterMoving.IsReadyToMove = true;
        _player.GetComponent<Battle>().AllowBattle = true;
        button1.gameObject.SetActive(false);
        button2.gameObject.SetActive(false);
        button3.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _entranceToTavern = GameObject.FindGameObjectWithTag("BuildingsImportant").GetComponent<ImportantBuildings>().EntranceToTavern.transform;
        tr = new GameObject().transform;
        _helpTel = true;
        _helpQue = true;
        _help3 = true;
        _help6 = true;
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
            default:
                break;
        }
    }

    private IEnumerator ShowSubtitles(int node)
    {
        _coroutSS = false;
        Node nodes = _dialogue4.nodes[node];
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
        Node nodes = _dialogue4.nodes[node];
        _startCoroutineSC = true;
        _target.text = "";
        _targetPoint.target = null;
        _groupCamera.enabled = true;
        CharacterMoving.IsReadyToMove = false;
        _player.GetComponent<Battle>().AllowBattle = false;
        StartCoroutine(RotateToTarget());
        foreach (Subtitles subt in nodes.npcText)
        {
            _target.text = "";
            _subtitles.text = subt.name + ": ";
            _subtitles.text += subt.text;
            yield return new WaitForSeconds(4f);
        }
        _subtitles.text = "";
        _target.text = "";
        _startCoroutineSC = false;
        _groupCamera.enabled = false;
        CharacterMoving.IsReadyToMove = true;
        _player.GetComponent<Battle>().AllowBattle = true;
        if (nextQuest)
        {
            if (_questManag.resultGame == 1 || _questManag.resultGame == 3)
            {
                subquest = (Subquest)5;
                Invoke("EndGame", 20f);
            }
            else
            {
                if (subquest == (Subquest)3)
                {
                    Invoke("Helper", 3f);
                }
                else
                {
                    subquest = (Subquest)7;
                }
            }
        }
    }
    private void Helper()
    {
        _playerCharact.Teleport();
        _targetPoint.PointToTarget(partisans[0].transform);
        Invoke("TelepP", 3f);
    }
    private void ChangeCompanion(GameObject companion)
    {
        _targetGroup.m_Targets = new Cinemachine.CinemachineTargetGroup.Target[] { new Cinemachine.CinemachineTargetGroup.Target { target = GameObject.FindGameObjectWithTag("HeadPlayer").transform, weight = 1f, radius = 0f }, new Cinemachine.CinemachineTargetGroup.Target { target = companion.transform.GetChild(0).transform, weight = 1f, radius = 0f } };
        _targetDialogue = companion;
    }

    #region Задания
    private void SubQ1()
    {
        if (_coroutSS)
        {
            StartCoroutine(ShowSubtitles(0));
            switch (_questManag.resultGame)
            {
                case 1:
                    _target.text = "Пройти по дороге к началу леса";
                    foreach (var item in enemySoldiers)
                    {
                        // телепортируем в нужное место
                        item.GetComponent<NavMeshAgent>().enabled = false;
                        item.transform.position = new Vector3(959f, 0f, 329f) + new Vector3(Random.Range(-3, 3), 0f, Random.Range(-3, 3));
                        item.GetComponent<NavMeshAgent>().enabled = true;
                        item.GetComponent<NavMeshAgent>().SetDestination(item.transform.position);
                    }
                    tr = enemySoldiers[0].transform;
                    break;
                case 2:
                    _target.text = "Подойти к заднему двору таверны";
                    foreach (var item in partisans)
                    {
                        Transform tr1 = new GameObject().transform;
                        tr1.position = new Vector3(946f, 8f, 767f);
                        item.GetComponent<Hunter>().TeleportTo(tr1);
                        item.GetComponent<NavMeshAgent>().SetDestination(item.transform.position);
                    }
                    tr = partisans[0].transform;
                    break;
                case 3:
                    _target.text = "Подойти к мосту через реку";
                    foreach (var item in allySoldiers)
                    {
                        item.GetComponent<Enemy>().control = true;
                        item.GetComponent<NavMeshAgent>().enabled = false;
                        item.transform.position = new Vector3(822f, 0f, 1157f) + new Vector3(Random.Range(-3, 3), 0f, Random.Range(-3, 3));
                        item.GetComponent<NavMeshAgent>().enabled = true;
                        item.GetComponent<NavMeshAgent>().SetDestination(item.transform.position);
                    }
                    tr = allySoldiers[0].transform;
                    break;
                default:
                    break;
            }
            _targetPoint.PointToTarget(tr);
        }
        if (Vector3.Distance(_player.transform.position, tr.position) < 2f)
        {
            switch (_questManag.resultGame)
            {
                case 1:
                    subquest = (Subquest)2;
                    ChangeCompanion(enemySoldiers[0]);
                    break;
                case 2:
                    subquest = (Subquest)3;
                    ChangeCompanion(partisans[0]);
                    break;
                case 3:
                    subquest = (Subquest)4;
                    ChangeCompanion(allySoldiers[0]);
                    break;
                default:
                    break;
            }
        }
    }
    private void SubQ2() // полохая: вражеская армия
    {
        if (!_startCoroutineSC)
        {
            ChangeCompanion(enemySoldiers[0]);
            StartCoroutine(ShowCutscene(1));
        }
    }
    private void SubQ3() // стредняя: партизаны
    {
        if (!_startCoroutineSC)
        {
            ChangeCompanion(partisans[0]);
            if (_help3)
            {
                _help3 = false;
                StartCoroutine(ShowCutscene(2));
            }
        }

    }
    private void SubQ4() // хорошая: союзная армия
    {
        if (!_startCoroutineSC)
        {
            ChangeCompanion(allySoldiers[0]);
            StartCoroutine(ShowCutscene(3));
        }
    }
    private void SubQ5()
    {
        _target.text = "Отправляйтесь в деревню";
        if (!_startCoroutineSC)
        {
            _startCoroutineSC = true;
            if (_questManag.resultGame == 1)
            {
                foreach (var item in enemySoldiers)
                {
                    item.GetComponent<NavMeshAgent>().SetDestination(_entranceToTavern.transform.position);
                }
            }
            else
            {
                foreach (var item in allySoldiers)
                {
                    item.GetComponent<NavMeshAgent>().SetDestination(_entranceToTavern.transform.position);
                }
            }
        }
    }
    private void SubQ6()
    {
        _target.text = "Подойдите к партизанам";
        if (!_startCoroutineSC && Vector3.Distance(_player.transform.position, partisans[0].transform.position) < 2f)
        {
            _helpQue = false;
            StartCoroutine(ShowCutscene(4));
        }
    }
    private void TelepP()
    {
        subquest = (Subquest)6;
        int i = 0;
        foreach (var item in partisans)
        {
            ++i;
            Transform tr0 = new GameObject().transform;
            tr0.position = new Vector3(186f, -164f, -2243f); //GameObject.FindGameObjectWithTag("ForestCart").transform.position;
            tr0.position += new Vector3(Random.Range(-3, 3), 0f, Random.Range(-3, 3));
            item.GetComponent<Hunter>().TeleportTo(tr0);
        }
    }
    private void SubQ7()
    {
        if (_help6)
        {
            _help6 = false;
            Invoke("EndGame", 15f);
        }
    }
    #endregion

    private void EndGame() //черный фон, показ титров, потом в гл меню bkb gj[ ghjcnj d uk vty.
    {
        LevelLoader levelLoader = GameObject.FindGameObjectWithTag("LevelLoader").GetComponent<LevelLoader>();
        levelLoader.PlayScene("MainMenu");
    }

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
[XmlRoot("quest4")]
public class Dialogue4
{
    [XmlElement("node")]
    public Node[] nodes;

    public static Dialogue4 Load(TextAsset _xml)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Dialogue4));
        StringReader reader = new StringReader(_xml.text);
        Dialogue4 dial = serializer.Deserialize(reader) as Dialogue4;
        return dial;
    }
}