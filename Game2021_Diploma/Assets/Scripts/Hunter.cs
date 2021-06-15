using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Hunter : MonoBehaviour
{
    public float hp;
    public bool die;
    private bool _agressive;
    private Animator _animator;
    private NavMeshAgent _agent;
    private AudioSource _audioSource;

    private GameObject[] _allForestAnimals;
    private SpawnEnemyes _enemies;
    private GameObject _arrow;
    private AudioClip[] _arrowSound;

    private float _speedRun;
    private float _speedWalk;
    private bool _shot;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _audioSource = GetComponent<AudioSource>();

        _speedRun = Random.Range(3.0f, 4.0f);
        _speedWalk = Random.Range(1.5f, 2.5f);
        _shot = false;
        _agressive = false;
        die = false;
        StartCoroutine(FindAndAttackAnimals());

        _enemies = GameObject.FindGameObjectWithTag("Enemies").GetComponent<SpawnEnemyes>();
        ++_enemies.allEnemies["Partisans"];
        _allForestAnimals = _enemies.allAnimals;
        _arrow = _enemies.arrow;
        _arrowSound = _enemies.arrowSound;
    }

    void Update()
    {

        if (_agent.velocity.magnitude > 0f)
        {
            if (_agent.speed == _speedWalk)
            {
                _animator.SetBool("Walk", true);
                _animator.SetBool("Run", false);
            }
            else if (_agent.speed == _speedRun)
            {
                _animator.SetBool("Run", true);
                _animator.SetBool("Walk", false);
            }
        }
        else
        {
            _animator.SetBool("Run", false);
            _animator.SetBool("Walk", false);
        }

        if (_agressive)
        {
            Attack(NearAnimals());
        }
    }

    public void Attack(Transform target)
    {
        if (Vector3.Distance(transform.position, target.position) > Random.Range(8.0f, 15.0f))
        {
            RunTo(target);
        }
        else
        {
            _shot = true;
            _agent.isStopped = true;
            StartCoroutine(RotateToTarget(target));
            _animator.SetTrigger("Shot");
        }
    }

    private IEnumerator RotateToTarget(Transform target)
    {
        while (_shot)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
            yield return null;
        }
    }

    public void WalkTo(Transform target)
    {
        _agent.speed = _speedWalk;
        if (Vector3.Distance(transform.position, target.position) < 2f)
        {
            _agent.SetDestination(transform.position);
        }
        else
        {
            _agent.SetDestination(target.position);
        }
    }
    public void RunTo(Transform target)
    {
        _agent.speed = _speedRun;
        _agent.SetDestination(target.position);
    }
    public void TeleportTo(Transform target)
    {
        transform.position = target.position;
    }
    public void Agressive(bool agr)
    {
        _agressive = agr;
    }

    private Transform NearAnimals()
    {
        float distance = Random.Range(9f, 11f);
        Transform target = _allForestAnimals[0].transform;
        for (int i = 0; i < _allForestAnimals.Length; i++)
        {
            if (Vector3.Distance(transform.position, _allForestAnimals[i].transform.position) < distance && Vector3.Distance(transform.position, _allForestAnimals[i].transform.position) < Vector3.Distance(transform.position, target.position))
            {
                target = _allForestAnimals[i].transform;
            }
        }
        return target;
    }
    private IEnumerator FindAndAttackAnimals()
    {
        while (true)
        {
            if (_agressive)
            {
                Attack(NearAnimals());
            }
            yield return new WaitForSeconds(10.0f);
        }
    }


    private void ShotArrow()
    {
        _shot = false;
        _agent.isStopped = false;

        _audioSource.pitch = Random.Range(0.9f, 1.1f);
        _audioSource.PlayOneShot(_arrowSound[Random.Range(0, _arrowSound.Length)]);

        GameObject newArrow = Instantiate(_arrow, transform.forward + new Vector3(0f, 1.0f, 0f), transform.rotation);
        //newArrow.transform.LookAt(_target);
        newArrow.GetComponent<Rigidbody>().useGravity = false;
        newArrow.GetComponent<Rigidbody>().velocity = newArrow.transform.forward * 10;
    }
}