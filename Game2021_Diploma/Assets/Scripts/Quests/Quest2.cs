using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest2 : MonoBehaviour
{
    public GameObject hunter1;
    public GameObject hunter2;
    public GameObject hunter3;
    public GameObject hunter4;

    private GameObject _player;


    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        Debug.Log("Квест 2!");
    }
}
