using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ForestAnimal : MonoBehaviour
{
    public float hp;
    private float _hpToRunAway;
    private float _defaultHp;
    public TypeAnimal animal;

    private Animator _animator;
    private NavMeshAgent _agent;
    private PlayerCharacteristics _playerCharact;

    private GameObject _player;
    private GameObject[] _hunters;

    public AudioClip[] roar;
    private AudioSource _audioSource;

    public bool _die = false;
    public bool _agressive = false;
    private bool _startCoroutine = false;
    private bool _startCoroutineW = false;
    private bool _startCoroutineE = false;
    private bool _walkCorout;
    private bool _attack;

    private float _speedWalk;
    private float _speedRun;
    private float _timeToEat;
    private bool _nextPlaces = false;
    private Transform _place;
    private float _timeToWalk;

    public GameObject[] places;
    private Animals _animals;
    private string _type = "";

    private List<AnimalLimbs> _limbs;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _hunters = GameObject.FindGameObjectsWithTag("Hunter");
        _playerCharact = _player.GetComponent<PlayerCharacteristics>();
        _place = places[Random.Range(0, places.Length)].transform;
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = 1.0f;
        _limbs = new List<AnimalLimbs>();
        _limbs.AddRange(GetComponentsInChildren<AnimalLimbs>());
        for (int i = 0; i < _limbs.Count; i++)
        {
            _limbs[i].parent = gameObject;
            _limbs[i].typeParent = AnimalLimbs.ParentAnimal.ForestAnimal;
        }

        switch (animal)
        {
            case TypeAnimal.Boar:
                hp = 400;
                _hpToRunAway = 300;
                _speedWalk = 1.0f;
                _speedRun = 3.0f;
                _type = "Boar";
                break;
            case TypeAnimal.Ibex:
                hp = 350;
                _hpToRunAway = 200;
                _speedWalk = 3.5f;
                _speedRun = 6.0f;
                _type = "Ibex";
                break;
            case TypeAnimal.Deer:
                hp = 450;
                _hpToRunAway = 300;
                _speedWalk = 2.0f;
                _speedRun = 4.0f;
                _type = "Deer";
                break;
            case TypeAnimal.Wolf:
                hp = 450;
                _hpToRunAway = 150;
                _speedWalk = 2.0f;
                _speedRun = 5.0f;
                _type = "Wolf";
                break;
            default:
                break;
        }
        _animals = GameObject.FindGameObjectWithTag("Animal").GetComponent<Animals>();
        ++_animals.allAnimals[_type];
        StartCoroutine(Healing());
    }

    void Update()
    {
        if (_die) { return; }
        if (hp <= 0)
        {
            --_animals.allAnimals[_type];
            _die = true;
            _agressive = false;
            _audioSource.Stop();
            _audioSource.enabled = false;
            _playerCharact.allAnimals.Remove(gameObject);
            _agent.enabled = false;
            _animator.SetTrigger("Die");
            Invoke("Delete", 300.0f);
            return;
        }

        if (!_audioSource.isPlaying)
        {
            _audioSource.pitch = Random.Range(0.9f, 1.1f);
            _audioSource.clip = roar[Random.Range(0, roar.Length)];
            _audioSource.Play();
        }


        if (Vector3.Distance(transform.position, _player.transform.position) < SafetyDistance() || NearHunters())
        {
            _agressive = true;
            _agent.speed = _speedRun;
        }
        else
        {
            _agressive = false;
            _agent.speed = _speedWalk;
        }

        if (_agressive)
        {
            _agent.speed = _speedRun;
            if (!_playerCharact.allAnimals.Contains(gameObject))
            {
                _playerCharact.allAnimals.Add(gameObject);
            }
        }
        else
        {
            _agent.speed = _speedWalk;
            _playerCharact.allAnimals.Remove(gameObject);
        }

        if (_agent.velocity.magnitude > 0f)
        {
            if (_agressive)
            {
                _animator.SetBool("Walk", false);
                _animator.SetBool("Eat", false);
                _animator.SetBool("Run", true);
            }
            else
            {
                _animator.SetBool("Run", false);
                _animator.SetBool("Eat", false);
                _animator.SetBool("Walk", true);
            }
        }
        else
        {
            if (!_startCoroutineE)
            {
                _animator.SetBool("Walk", false);
                _animator.SetBool("Run", false);
                _animator.SetBool("Eat", false);
            }
            if (!_agressive && !_startCoroutineE && Time.time - _timeToEat >= Random.Range(60.0f, 140.0f))
            {
                _timeToEat = Time.time;
                StartCoroutine(Eat());
            }
        }

        if (_agressive && hp <= 250 && Vector3.Distance(transform.position, places[places.Length - 1].transform.position) > 7.5f) { RunAway(); }
        else if (_agressive) { Attack(); }
        else { Walking(); }
    }
    private bool NearHunters()
    {
        float distance = Random.Range(9f, 11f);
        for (int i = 0; i < _hunters.Length; i++)
        {
            if (Vector3.Distance(transform.position, _hunters[i].transform.position) < distance)
            {
                return true;
            }
        }
        return false;
    }
    private float SafetyDistance()
    {
        float distance = Random.Range(9f, 11f);
        if (_playerCharact.crouch)
        {
            return distance;
        }
        return distance * 2f;
    }
    private void Attack()
    {
        _walkCorout = false;
        Transform enHit = EnemyForHit().transform;
        if (Vector3.Distance(transform.position, enHit.position) > 1.5f)
        {
            _attack = false;
            _agent.SetDestination(enHit.position);
        }
        else
        {
            _attack = true;
            if (!_startCoroutine)
            {
                StartCoroutine(Hit());
            }
        }
    }
    private Transform EnemyForHit()
    {
        Transform enfh = _player.transform;
        for (int i = 0; i < _hunters.Length; i++)
        {
            if (Vector3.Distance(transform.position, enfh.position) > Vector3.Distance(transform.position, _hunters[i].transform.position))
            {
                enfh = _hunters[i].transform;
            }
        }
        return enfh;
    }
    IEnumerator Hit()
    {
        _agent.isStopped = true;
        _startCoroutine = true;
        while (_attack)
        {
            _animator.SetTrigger("Attack");
            yield return new WaitForSeconds(Random.Range(2.0f, 2.7f));
        }
        _agent.isStopped = false;
        _startCoroutine = false;
    }

    private void Walking()
    {
        _walkCorout = true;
        if (Vector3.Distance(_place.position, transform.position) < 2.0f)
        {
            if (Time.time - _timeToWalk >= Random.Range(3.0f, 30.0f))
            {
                _nextPlaces = true;
            }
        }
        else
        {
            _timeToWalk = Time.time;
            _agent.SetDestination(_place.position);
        }
        if (!_startCoroutineW)
        {
            StartCoroutine(Walk());
        }
    }
    IEnumerator Walk()
    {
        _startCoroutineW = true;
        while (_walkCorout)
        {
            _place = places[Random.Range(0, places.Length)].transform;
            yield return new WaitUntil(() => _nextPlaces);
            _nextPlaces = false;
        }
        _startCoroutineW = false;
    }
    IEnumerator Eat()
    {
        _startCoroutineE = true;
        _animator.SetBool("Eat", true);
        yield return new WaitForSeconds(Random.Range(5f, 15f));
        _animator.SetBool("Eat", false);
        _startCoroutineE = false;
    }

    private void RunAway()
    {
        _attack = false;
        _agent.SetDestination(places[places.Length - 1].transform.position);
    }
    public void Agressive()
    {
        if (!_die)
        {
            if (NearHunters())
            {
                _agressive = true;
            }
            else
            {
                RunAway();
            }
        }
    }

    IEnumerator Healing()
    {
        while (!_die)
        {
            if (hp < 500)
            {
                hp += 20;
            }
            yield return new WaitForSeconds(10f);
        }
    }
    private void Delete()
    {
        Destroy(gameObject);
    }
    public enum TypeAnimal
    {
        Wolf,
        Boar,
        Ibex,
        Deer,
    }
}