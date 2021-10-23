using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImportantBuildings : MonoBehaviour
{
    public GameObject Shop1;
    public GameObject Shop2;
    public GameObject Shop3;
    public GameObject Shop4;
    public GameObject Shop5;
    public GameObject EntranceToTavern;
    public GameObject Garden;
    public GameObject RightGate;
    public GameObject LeftGate;
    public GameObject RightUpGate;
    public GameObject LeftUpGate;

    public GameObject[] allImportantBuildings;

    private void Start()
    {
        allImportantBuildings = new GameObject[] { Shop1, Shop2, Shop3, Shop4, Shop5, EntranceToTavern, Garden, RightGate, LeftGate, RightUpGate, LeftUpGate };
    }
}