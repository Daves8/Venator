using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    private Animator _animator;
    private NavMeshAgent _agent;

    private ImportantBuildings _importantBuildings;
    private GameObject[] _allBuildings;

    private bool _isWalk = true;
    private bool _isRun;

    public Build nextBuild;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _importantBuildings = GameObject.FindGameObjectWithTag("BuildingsImportant").GetComponent<ImportantBuildings>();
        _allBuildings = new GameObject[] { _importantBuildings.Shop1, _importantBuildings.Shop2, _importantBuildings.Shop3, _importantBuildings.Shop4, _importantBuildings.Shop5, _importantBuildings.EntranceToTavern, _importantBuildings.Garden, _importantBuildings.RightGate, _importantBuildings.RightUpGate, _importantBuildings.LeftUpGate };

        nextBuild = (Build)Random.Range(1, 11);
    }

    // Update is called once per frame
    void Update()
    {
        if (_agent.velocity.normalized.magnitude >= 0.1f)
        {
            //StopCoroutine("AnimIdle");
            _animator.SetBool("Walk", true);
        }
        else
        {
            _animator.SetBool("Walk", false);
        }

        if (nextBuild != Build.nothing)
        {
            _agent.SetDestination(_allBuildings[(int)nextBuild - 1].transform.position); //_importantBuildings.Shop1.transform.position);
        }



        if (_isWalk)
        {
            _agent.speed = 1.5f; //Random.Range(5f, 15f) / 10.0f;
            _isRun = false;
        }
        if (_isRun)
        {
            _agent.speed = 4f;
            _isWalk = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine("NpcAI");
        switch (other.gameObject.name)
        {
            case "Shop1":
                _agent.isStopped = true;

                break;
            case "Shop2":

                break;
            case "Shop3":

                break;
            case "Shop4":

                break;
            case "Shop5":

                break;
            case "EntranceToTavern":

                break;
            case "Garden":

                break;
            case "RightGate":

                break;
            case "RightUpGate":

                break;
            case "LeftUpGate":

                break;
            default:
                break;
        }
    }

    IEnumerator NpcAI()
    {
        yield return new WaitForSeconds(Random.Range(5f, 25f));
        nextBuild = (Build)Random.Range(1, 11);
    }

    IEnumerator AnimIdle()
    {
        _animator.SetTrigger("Thinking");
        yield return new WaitForSeconds(3f);
    }

    public enum Build
    {
        nothing,
        Shop1,
        Shop2,
        Shop3,
        Shop4,
        Shop5,
        EntranceToTavern,
        Garden,
        RightGate,
        RightUpGate,
        LeftUpGate
    }
}