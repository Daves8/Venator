using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VillageAnimal : MonoBehaviour
{
    public bool _die;
    private Animator _animator;
    private NavMeshAgent _agent;
    private AudioSource _audioSource;

    public AudioClip[] sounds;
    public TypeAnimalVillage animal;
    private Animals _animals;
    private string _type;
    private PlayerCharacteristics _playerCharact;
    public Transform[] places;
    private Transform _place;
    private bool _startCoroutineW;
    private bool _startCoroutineE;
    private bool _walkCorout;
    private bool _nextPlace;
    private float _timeToWalk;
    private float _timeToEat;
    public string testPlace;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = 0.5f;
        _animals = GameObject.FindGameObjectWithTag("Animal").GetComponent<Animals>();
        _playerCharact = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacteristics>();
        _place = places[Random.Range(0, places.Length)];
        _startCoroutineW = false;
        _startCoroutineE = false;
        _nextPlace = false;

        switch (animal)
        {
            case TypeAnimalVillage.Chicken:
                _type = "Chicken";
                _agent.speed = Random.Range(0.7f, 1.5f);
                break;
            case TypeAnimalVillage.Cattle:
                _type = "Cattle";
                _agent.speed = Random.Range(1.0f, 2.5f);
                break;
            case TypeAnimalVillage.Pig:
                _type = "Pig";
                _agent.speed = Random.Range(1.0f, 2.5f);
                break;
            default:
                break;
        }
        ++_animals.allAnimals[_type];
    }

    void Update()
    {
        if (_die) { return; }
        testPlace = _place.name;
        if (!_audioSource.isPlaying)
        {
            _audioSource.pitch = Random.Range(0.9f, 1.1f);
            _audioSource.clip = sounds[Random.Range(0, sounds.Length)];
            _audioSource.Play();
        }

        if (_agent.velocity.magnitude > 0.0f)
        {
            _animator.SetBool("Walk", true);
            _animator.SetBool("Eat", false);
        }
        else
        {
            if (!_startCoroutineE)
            {
                _animator.SetBool("Walk", false);
                _animator.SetBool("Eat", false);
            }
            if (!_startCoroutineE && Time.time - _timeToEat >= Random.Range(60.0f, 140.0f))
            {
                _timeToEat = Time.time;
                StartCoroutine(EatCorout());
            }
        }
        Walking();
    }

    private void Walking()
    {
        _walkCorout = true;
        if (Vector3.Distance(new Vector3(_place.position.x, 0f, _place.position.z), new Vector3(transform.position.x, 0f, transform.position.z)) <= 2.0f)
        {
            if (Time.time - _timeToWalk >= Random.Range(3.0f, 30.0f))
            {
                _nextPlace = true;
            }
        }
        else
        {
            _timeToWalk = Time.time;
            _agent.SetDestination(_place.position);
        }
        if (!_startCoroutineW)
        {
            StartCoroutine(WalkCorout());
        }
    }
    IEnumerator WalkCorout()
    {
        _startCoroutineW = true;
        while (_walkCorout)
        {
            _place = places[Random.Range(0, places.Length)].transform;
            yield return new WaitUntil(() => _nextPlace);
            _nextPlace = false;
        }
        _startCoroutineW = false;
    }
    IEnumerator EatCorout()
    {
        _startCoroutineE = true;
        _animator.SetBool("Eat", true);
        yield return new WaitForSeconds(Random.Range(5f, 15f));
        _animator.SetBool("Eat", false);
        _startCoroutineE = false;
    }

    private void Dying()
    {
        _die = true;
        if (!_playerCharact.allAnimals.Contains(gameObject))
        {
            _playerCharact.allAnimals.Add(gameObject);
        }
        Invoke("RemoveAn", 5.0f);
        --_animals.allAnimals[_type];
        _audioSource.Stop();
        _audioSource.enabled = false;
        _agent.enabled = false;
        _animator.SetTrigger("Die");
        Invoke("Delete", 300.0f);
    }
    private void RemoveAn()
    {
        _playerCharact.allAnimals.Remove(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Arrow")
        {
            Dying();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Knife" || other.tag == "Sword")
        {
            Dying();
        }
    }

    private void Delete()
    {
        Destroy(gameObject);
    }

    public enum TypeAnimalVillage
    {
        Chicken,
        Cattle,
        Pig
    }
}