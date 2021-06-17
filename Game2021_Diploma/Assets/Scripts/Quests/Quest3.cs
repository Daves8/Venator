using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Quest3 : MonoBehaviour
{
    private GameObject _player;
    private Player _scriptPlayer;
    private PlayerCharacteristics _playerCharact;

    private QuestsManagement _questManag;
    private Dialogue3 _dialogue3;

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

    private GameObject _mother;
    private GameObject _father;
    private GameObject _innkeeper;
    private GameObject _seller;

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
        _questManag = GetComponent<QuestsManagement>();
        _mother = _questManag.mother;
        _father = _questManag.father;
        _innkeeper = _questManag.innkeeper;
        _seller = _questManag.seller;
        _dialogue3 = Dialogue3.Load(_questManag.dialoguesQ3);
        _targetDialogue = _innkeeper;
        _groupCamera.enabled = false;
        _targetGroup.m_Targets = new Cinemachine.CinemachineTargetGroup.Target[] { new Cinemachine.CinemachineTargetGroup.Target { target = GameObject.FindGameObjectWithTag("HeadPlayer").transform, weight = 1f, radius = 0f }, new Cinemachine.CinemachineTargetGroup.Target { target = _targetDialogue.transform.GetChild(0).transform, weight = 1f, radius = 0f } };

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
        Node nodes = _dialogue3.nodes[node];
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
        Node nodes = _dialogue3.nodes[node];
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
            yield return new WaitForSeconds(3f);
        }
        _subtitles.text = "";
        _target.text = "";
        _startCoroutineSC = false;
        _groupCamera.enabled = false;
        CharacterMoving.IsReadyToMove = true;
        _player.GetComponent<Battle>().AllowBattle = true;
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
        if (!_startCoroutineSC)
        {
            if (_questManag.resultQuests[1] == 1)
            {
                StartCoroutine(ShowCutscene(0));
            }
            else if (_questManag.resultQuests[1] == 2)
            {
                StartCoroutine(ShowCutscene(1));
            }
            else if (_questManag.resultQuests[1] == 3)
            {
                StartCoroutine(ShowCutscene(2));
            }
            // убрать шкуры из инвентаря и дать золото
            int countSkinBoar = _scriptPlayer.inventory.FindItemOnInventory(6);
            _playerCharact.gold += countSkinBoar * 75;
            _scriptPlayer.inventory.FindItemOnInventory(new Item(_scriptPlayer.dbVenator.ItemObjects[6])).RemoveItem();
        }
    }
    private void SubQ2()
    {
        _target.text = "Купить у торговца молока";
        _targetPoint.PointToTarget(_seller.transform);
        if(!_startCoroutineSC && Vector3.Distance(_player.transform.position, _seller.transform.position) < 2f)
        {
            ChangeCompanion(_seller);
            StartCoroutine(ShowCutscene(3));
            _playerCharact.gold -= 30;
            _scriptPlayer.inventory.AddItem(new Item(_scriptPlayer.dbVenator.ItemObjects[15]), 1);
        }
    }
    private void SubQ3()
    {
        _target.text = "Отдать молоко корчмарю";
        _targetPoint.PointToTarget(_innkeeper.transform);
        if (!_startCoroutineSC && Vector3.Distance(_player.transform.position, _innkeeper.transform.position) < 2f)
        {
            ChangeCompanion(_innkeeper);
            StartCoroutine(ShowCutscene(4));
            _scriptPlayer.inventory.FindItemOnInventory(new Item(_scriptPlayer.dbVenator.ItemObjects[15])).AddAmount(-1);
        }
    }
    private void SubQ4()
    {
        _target.text = "Подойти к отцу";
        _targetPoint.PointToTarget(_father.transform);
        if (!_startCoroutineSC && Vector3.Distance(_player.transform.position, _father.transform.position) < 2f)
        {
            _scriptPlayer.SavePlayer();
            _playerCharact.gold -= 60;
            ChangeCompanion(_father);
            float resultGameHlp = (_questManag.resultQuests[0] + _questManag.resultQuests[1]) / 2;
            if (resultGameHlp != resultGameHlp.ToString()[0])
            {
                if (resultGameHlp > resultGameHlp.ToString()[0])
                {
                    _questManag.resultGame = (int)(resultGameHlp - 0.5f);
                }
                else if (resultGameHlp < resultGameHlp.ToString()[0])
                {
                    _questManag.resultGame = (int)(resultGameHlp + 0.5f);
                }
            }
            else
            {
                _questManag.resultGame = (int)resultGameHlp;
            }
            switch (_questManag.resultGame)
            {
                case 1:
                    StartCoroutine(ShowCutscene(5));
                    _scriptPlayer.inventory.AddItem(new Item(_scriptPlayer.dbVenator.ItemObjects[18]), 1);
                    break;
                case 2:
                    StartCoroutine(ShowCutscene(6));
                    _scriptPlayer.inventory.AddItem(new Item(_scriptPlayer.dbVenator.ItemObjects[19]), 1);
                    break;
                case 3:
                    StartCoroutine(ShowCutscene(7));
                    _scriptPlayer.inventory.AddItem(new Item(_scriptPlayer.dbVenator.ItemObjects[19]), 1);
                    _scriptPlayer.inventory.AddItem(new Item(_scriptPlayer.dbVenator.ItemObjects[1]), 1);
                    break;
                default:
                    Debug.LogError("НЕВОЗМОЖНО! Ошибка в просчете результатов игры. 281 строка 3 квеста");
                    break;
            }
        }
    }
    private void SubQ5()
    {
        // получили экипировку, наверное новый (последний) квест
        _questManag.quest = Quest.quest4;
    }
    private void SubQ6()
    {
        
    }
    private void SubQ7()
    {
        
    }
    private void SubQ8()
    {
        
    }
    private void SubQ9()
    {
        
    }
    private void SubQ10()
    {
        
    }
    private void SubQ11()
    {
        
    }
    private void SubQ12()
    {
        
    }
    private void SubQ13()
    {
        
    }
    private void SubQ14()
    {
       
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
[XmlRoot("quest3")]
public class Dialogue3
{
    [XmlElement("node")]
    public Node[] nodes;

    public static Dialogue3 Load(TextAsset _xml)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Dialogue3));
        StringReader reader = new StringReader(_xml.text);
        Dialogue3 dial = serializer.Deserialize(reader) as Dialogue3;
        return dial;
    }
}