using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animals : MonoBehaviour
{
    public Dictionary<string, int> allAnimals;
    public Dictionary<string, int> _allAnimalsCount;
    private string[] _allAnimalsName;
    private GameObject[] _allAnimalsObject;
    private bool _init = true;

    public Transform[] spawnAnimalForest;
    public Transform[] spawnAnimalVillage;

    // Forest
    public GameObject Wolf;
    public GameObject[] Rabbit;
    public GameObject Boar;
    public GameObject Ibex;
    public GameObject[] Deer;
    public GameObject Bear;
    public GameObject Viper;

    // Village
    public GameObject[] Chicken;
    public GameObject[] Cattle;
    public GameObject Rat;
    public GameObject[] Pig;
    public GameObject Goat;

    void Awake()
    {
        allAnimals = new Dictionary<string, int>();
        _allAnimalsCount = new Dictionary<string, int>();
        _allAnimalsName = new string[] { "Wolf", "Rabbit", "Boar", "Ibex", "Deer", "Bear", "Viper", "Chicken", "Cattle", "Rat", "Pig", "Goat" };
        for (int i = 0; i < _allAnimalsName.Length; i++)
        {
            allAnimals.Add(_allAnimalsName[i], 0);
        }
        //allAnimals.Add("Wolf", 0);
        //allAnimals.Add("Rabbit", 0);
        //allAnimals.Add("Boar", 0);
        //allAnimals.Add("Ibex", 0);
        //allAnimals.Add("Deer", 0);
        //allAnimals.Add("Bear", 0);
        //allAnimals.Add("Viper", 0);

        //allAnimals.Add("Chicken", 0);
        //allAnimals.Add("Cattle", 0);
        //allAnimals.Add("Rat", 0);
        //allAnimals.Add("Pig", 0);
        //allAnimals.Add("Goat", 0);
    }

    void Update()
    {
        if (_init)
        {
            _init = false;
            Initialize();
        }

        for (int i = 0; i < _allAnimalsName.Length; i++)
        {
            if (allAnimals[_allAnimalsName[i]] < _allAnimalsCount[_allAnimalsName[i]] * 0.75)
            {
                switch (_allAnimalsName[i])
                {
                    case "Rabbit":
                        _allAnimalsObject[i] = Rabbit[Random.Range(0, Rabbit.Length)];
                        break;
                    case "Deer":
                        _allAnimalsObject[i] = Deer[Random.Range(0, Deer.Length)];
                        break;
                    case "Chicken":
                        _allAnimalsObject[i] = Chicken[Random.Range(0, Chicken.Length)];
                        break;
                    case "Cattle":
                        _allAnimalsObject[i] = Cattle[Random.Range(0, Cattle.Length)];
                        break;
                    case "Pig":
                        _allAnimalsObject[i] = Pig[Random.Range(0, Pig.Length)];
                        break;
                    default:
                        break;
                }
                if (i < 7) // forest
                {
                    Instantiate(_allAnimalsObject[i], spawnAnimalForest[Random.Range(0, spawnAnimalForest.Length)].position, Quaternion.identity);
                }
                else // village
                {
                    Instantiate(_allAnimalsObject[i], spawnAnimalVillage[Random.Range(0, spawnAnimalVillage.Length)].position, Quaternion.identity);
                }
            }
        }
    }

    private void Initialize()
    {
        _allAnimalsObject = new GameObject[] { Wolf, Rabbit[0], Boar, Ibex, Deer[0], Bear, Viper, Chicken[0], Cattle[0], Rat, Pig[0], Goat };
        for (int i = 0; i < _allAnimalsName.Length; i++)
        {
            _allAnimalsCount.Add(_allAnimalsName[i], allAnimals[_allAnimalsName[i]]);

        }
    }
}