using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestsManagement : MonoBehaviour
{
    public TextAsset dialogues;

    public GameObject button1;
    public GameObject button2;
    public GameObject button3;

    public bool quest1complete;

    void Awake()
    {
        button1.SetActive(false);
        button2.SetActive(false);
        button3.SetActive(false);
    }

    void Update()
    {
        if (quest1complete)
        {
            GetComponent<Quest1>().enabled = false;
        }
    }
}
