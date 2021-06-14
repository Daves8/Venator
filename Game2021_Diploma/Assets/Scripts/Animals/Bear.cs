using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bear : MonoBehaviour
{
    public float hp;
    public bool loot;

    private Animator _bearAnim;
    private NavMeshAgent _bearAgent;
    private PlayerCharacteristics _playerCharact;

    private GameObject _player;
    private GameObject[] _hunters;

    public AudioClip[] roarBear;
    private AudioSource _audioSource;

    private bool _die = false;
    public bool _agressive = false;
    private bool _startCoroutine = false;
    private bool _startCoroutineW = false;
    private bool _startCoroutineE = false;
    private bool _walkCorout;
    private bool _attack;
    private float _timeToEat;
    private bool _nextPlaces = false;
    private Transform _place;
    private float _timeToWalk;

    private float _speedWalk = 1.5f;
    private float _speedRun = 4.0f;

    public GameObject[] _places;
    private Animals _animals;

    private List<AnimalLimbs> _limbs;

    private bool _checkState;
    void Start()
    {
        _bearAnim = GetComponent<Animator>();
        _bearAgent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _hunters = GameObject.FindGameObjectsWithTag("Hunter");
        _playerCharact = _player.GetComponent<PlayerCharacteristics>();
        _checkState = true;
        _places[_places.Length - 1] = GameObject.FindGameObjectWithTag("Den");
        _place = _places[Random.Range(0, _places.Length)].transform;
        _animals = GameObject.FindGameObjectWithTag("Animal").GetComponent<Animals>();
        ++_animals.allAnimals["Bear"];
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = 1.0f;
        hp = 750;
        loot = true;
        StartCoroutine(Healing());
        _limbs = new List<AnimalLimbs>();
        _limbs.AddRange(GetComponentsInChildren<AnimalLimbs>());
        for (int i = 0; i < _limbs.Count; i++)
        {
            _limbs[i].parent = gameObject;
            _limbs[i].typeParent = AnimalLimbs.ParentAnimal.Bear;
        }
        GetComponent<SphereCollider>().enabled = false;
    }

    void Update()
    {
        if (_die) { return; }
        if (hp <= 0)
        {
            --_animals.allAnimals["Bear"];
            _die = true;
            _agressive = false;
            _audioSource.Stop();
            _audioSource.enabled = false;
            _playerCharact.allAnimals.Remove(gameObject);
            _bearAgent.enabled = false;
            _bearAnim.SetTrigger("Die");
            Invoke("Delete", 300.0f);
            GetComponent<SphereCollider>().enabled = true;
            return;
        }

        if (!_audioSource.isPlaying)
        {
            _audioSource.pitch = Random.Range(0.9f, 1.1f);
            _audioSource.clip = roarBear[Random.Range(0, roarBear.Length)];
            _audioSource.Play();
        }

        if (_checkState)
        {
            _checkState = false;
            StartCoroutine(CheckState());
        }

        if (_agressive)
        {
            _bearAgent.speed = _speedRun;
            if (!_playerCharact.allAnimals.Contains(gameObject))
            {
                _playerCharact.allAnimals.Add(gameObject);
            }
        }
        else
        {
            _bearAgent.speed = _speedWalk;
            _playerCharact.allAnimals.Remove(gameObject);
        }

        if (_bearAgent.velocity.magnitude > 0f)
        {
            if (_agressive)
            {
                _bearAnim.SetBool("Walk", false);
                _bearAnim.SetBool("Eat", false);
                _bearAnim.SetBool("Run", true);
            }
            else
            {
                _bearAnim.SetBool("Run", false);
                _bearAnim.SetBool("Eat", false);
                _bearAnim.SetBool("Walk", true);
            }
        }
        else
        {
            if (!_startCoroutineE)
            {
                _bearAnim.SetBool("Walk", false);
                _bearAnim.SetBool("Run", false);
                _bearAnim.SetBool("Eat", false);
            }
            if (!_agressive && !_startCoroutineE && Time.time - _timeToEat >= Random.Range(60.0f, 140.0f))
            {
                _timeToEat = Time.time;
                StartCoroutine(Eat());
            }
        }

        if (_agressive && hp <= 250 && Vector3.Distance(transform.position, _places[_places.Length - 1].transform.position) > 7.5f) { RunAway(); }
        else if (_agressive) { Attack(); }
        else { Walking(); }
    }
    private IEnumerator CheckState()
    {
        while (!_die)
        {
            if (Vector3.Distance(transform.position, _player.transform.position) < SafetyDistance() || NearHunters())
            {
                _agressive = true;
                _bearAgent.speed = _speedRun;
            }
            else
            {
                _agressive = false;
                _bearAgent.speed = _speedWalk;
            }
            yield return new WaitForSeconds(5.0f);
        }
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
            _bearAgent.SetDestination(enHit.position);
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
        _bearAgent.isStopped = true;
        _startCoroutine = true;
        while (_attack)
        {
            _bearAnim.SetTrigger("Attack");
            yield return new WaitForSeconds(Random.Range(2.0f, 2.7f));
        }
        _bearAgent.isStopped = false;
        _startCoroutine = false;
    }

    private void Walking()
    {
        _walkCorout = true;
        if (Vector3.Distance(new Vector3(_place.position.x, 0f, _place.position.z), new Vector3(transform.position.x, 0f, transform.position.z)) <= 2.0f)
        {
            if (Time.time - _timeToWalk >= Random.Range(3.0f, 30.0f))
            {
                _nextPlaces = true;
            }
        }
        else
        {
            _timeToWalk = Time.time;
            _bearAgent.SetDestination(_place.position);
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
            _place = _places[Random.Range(0, _places.Length)].transform;
            
            yield return new WaitUntil(() => _nextPlaces);
            _nextPlaces = false;
        }
        _startCoroutineW = false;
    }
    IEnumerator Eat()
    {
        _startCoroutineE = true;
        _bearAnim.SetBool("Eat", true);
        yield return new WaitForSeconds(Random.Range(5f, 15f));
        _bearAnim.SetBool("Eat", false);
        _startCoroutineE = false;
    }

    private void RunAway()
    {
        _attack = false;
        _bearAgent.SetDestination(_places[_places.Length - 1].transform.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_die)
        {
            if (collision.gameObject.tag == "Arrow")
            {
                Agressive();
                hp -= Random.Range(30, 100);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!_die)
        {
            // охотник
            if (other.gameObject.tag == "SwordEn")
            {
                Agressive();
                hp -= Random.Range(30, 70);
            }
            else if (other.gameObject.tag == "KnifeEn")
            {
                Agressive();
                hp -= Random.Range(10, 30);
            }

            // игрок
            if (other.gameObject.tag == "Sword")
            {
                Agressive();
                hp -= Random.Range(30, 70); // 100-150 меч 2-го уровня
            }
            else if (other.gameObject.tag == "Knife")
            {
                Agressive();
                hp -= Random.Range(10, 30);
            }
        }
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
}