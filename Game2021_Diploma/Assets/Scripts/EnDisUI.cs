using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnDisUI : MonoBehaviour
{
    public GameObject[] ui;
    private bool _enUI;

    private void Start()
    {
        _enUI = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Slash))
        {
            _enUI = !_enUI;
            for (int i = 0; i < ui.Length; i++)
            {
                ui[i].SetActive(_enUI);
            }
        }
    }
}