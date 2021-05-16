using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rat : MonoBehaviour
{
    private Animator _animator;
    private NavMeshAgent _agent;
    private ImportantBuildings _importBuild;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _importBuild = GameObject.FindGameObjectWithTag("BuildingsImportant").GetComponent<ImportantBuildings>();

        StartCoroutine(Walk());
    }

    private void Update()
    {
        if (_agent.velocity.magnitude > 0f)
        {
            // крыса бежит
        }
        else
        {
            // крыса стоит
        }
    }

    IEnumerator Walk()
    {
        while (true)
        {
            _agent.SetDestination(_importBuild.allImportantBuildings[Random.Range(0, _importBuild.allImportantBuildings.Length)].transform.position);
            yield return new WaitForSeconds(Random.Range(5f, 180f));
        }
    }
}