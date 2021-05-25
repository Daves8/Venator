using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animals : MonoBehaviour
{
    public Dictionary<string, int> allAnimals;

    // Forest
    public GameObject Wolf;
    public GameObject[] Rabbit;
    public GameObject Boar;
    public GameObject Ibex;
    public GameObject[] Deer;
    public GameObject Bear;




    void Awake()
    {
        allAnimals = new Dictionary<string, int>();
        allAnimals.Add("Rat", 0);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
