using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Quest5 : MonoBehaviour
{
    private GameObject _player;
    private Player _scriptPlayer;
    private PlayerCharacteristics _playerCharact;

    private QuestsManagement _questManag;
    private Dialogue5 _dialogue5;

    private GameObject[] allySoldiers;
    private GameObject[] partisans;
    private GameObject[] enemySoldiers;

    private TextMeshProUGUI _subtitles;
    private TextMeshProUGUI _prompt;
    private TextMeshProUGUI _target;

    public BackgroundMusic backgroundMusic;

    private ImportantBuildings _importantBuildings;
    private FinalBattle _finalBattle;

    public Cinemachine.CinemachineVirtualCamera _groupCamera;
    private TargetPoint _targetPoint;

    private bool _coroutSS;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _scriptPlayer = _player.GetComponent<Player>();
        _questManag = GetComponent<QuestsManagement>();
        _dialogue5 = Dialogue5.Load(_questManag.dialoguesQ5);
        allySoldiers = GameObject.FindGameObjectsWithTag("Enemy");
        partisans = GameObject.FindGameObjectsWithTag("Hunter");
        enemySoldiers = GameObject.FindGameObjectsWithTag("EnemySoldier");
        _groupCamera.enabled = false;

        _finalBattle = GetComponent<FinalBattle>();

        _subtitles = _questManag.subtitles.GetComponent<TextMeshProUGUI>();
        _prompt = _questManag.prompt.GetComponent<TextMeshProUGUI>();
        _target = _questManag.target.GetComponent<TextMeshProUGUI>();
        _subtitles.text = "";
        _prompt.text = "";
        _target.text = "";
        _targetPoint = GetComponent<TargetPoint>();
        _playerCharact = _player.GetComponent<PlayerCharacteristics>();
        _importantBuildings = GameObject.FindGameObjectWithTag("BuildingsImportant").GetComponent<ImportantBuildings>();

        CharacterMoving.IsReadyToMove = true;
        _player.GetComponent<Battle>().AllowBattle = true;
        _questManag.button1.GetComponent<Button>().gameObject.SetActive(false);
        _questManag.button2.GetComponent<Button>().gameObject.SetActive(false);
        _questManag.button3.GetComponent<Button>().gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _coroutSS = true;
        backgroundMusic.finalBattle = true;

        foreach (var item in allySoldiers)
        {
            item.GetComponent<Enemy>().canWalkInVillage = false;
            item.GetComponent<NavMeshAgent>().enabled = false;
            item.transform.position = _importantBuildings.RightGate.transform.position + new Vector3(Random.Range(-4f, 4f), 0f, Random.Range(-4f, 4f) - 8f);
            item.transform.rotation = Quaternion.Euler(0f, Random.Range(-190, -210), 0f);
            item.GetComponent<NavMeshAgent>().enabled = true;
            item.GetComponent<NavMeshAgent>().SetDestination(item.transform.position);
        }

        foreach (var item in enemySoldiers)
        {
            item.GetComponent<Enemy>().canWalkInVillage = false;
            item.GetComponent<NavMeshAgent>().enabled = false;
            item.transform.position = _importantBuildings.LeftGate.transform.position + new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f));
            item.GetComponent<NavMeshAgent>().enabled = true;
            item.GetComponent<NavMeshAgent>().SetDestination(item.transform.position);
        }

        StartCoroutine(StartBattle());
    }

    private IEnumerator ShowSubtitles(int node)
    {
        _coroutSS = false;
        Node nodes = _dialogue5.nodes[node];
        foreach (Subtitles subt in nodes.npcText)
        {
            _subtitles.text = subt.name + ": ";
            _subtitles.text += subt.text;
            yield return new WaitForSeconds(4f);
        }
        _subtitles.text = "";
    }

    private IEnumerator StartBattle()
    {
        yield return new WaitForSeconds(2f);
        StartCoroutine(ShowSubtitles(0));
        //yield return new WaitForSeconds(2f);
        _finalBattle.GoToBattlePoints();
        yield return new WaitForSeconds(100f);
        for (int i = 0; i < 20; i++)
        {
            //_finalBattle.KillSomeEnemy();
            enemySoldiers[i].GetComponent<Enemy>()._hp = -100;
            if (i + 1 < 20)
            {
                allySoldiers[i].GetComponent<Enemy>()._player = enemySoldiers[i + 1];
            }
            if (i % 2 == 0)
            {
                allySoldiers[i].GetComponent<Enemy>()._hp = -100;
            }
            yield return new WaitForSeconds(Random.Range(0.5f, 2f));
        }

        _finalBattle.Final();
        yield return new WaitForSeconds(25f);
        for (int i = 20; i < 27; i++)
        {
            enemySoldiers[i].GetComponent<Enemy>()._hp = -100;
            yield return new WaitForSeconds(Random.Range(0.5f, 2f));
        }

        foreach (var item in allySoldiers)
        {
            //item.GetComponent<Enemy>().canWalkInVillage = true;
            item.GetComponent<Enemy>()._agressive = false;
            item.GetComponent<NavMeshAgent>().SetDestination(_importantBuildings.EntranceToTavern.transform.position + new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f)));
        }

        StartCoroutine(ShowSubtitles(1));
        print("КОНКЕЦ!!!");
        // конец игры через ...
        yield return new WaitForSeconds(8);
        _player.GetComponent<Player>().GEnding();
        yield return new WaitForSeconds(25f);
        LevelLoader levelLoader = GameObject.FindGameObjectWithTag("LevelLoader").GetComponent<LevelLoader>();
        levelLoader.PlayScene("MainMenu");
    }

    void Update()
    {





    }
}

[XmlRoot("quest5")]
public class Dialogue5
{
    [XmlElement("node")]
    public Node[] nodes;

    public static Dialogue5 Load(TextAsset _xml)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Dialogue5));
        StringReader reader = new StringReader(_xml.text);
        Dialogue5 dial = serializer.Deserialize(reader) as Dialogue5;
        return dial;
    }
}