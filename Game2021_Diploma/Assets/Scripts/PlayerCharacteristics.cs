﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacteristics : MonoBehaviour
{
    // скрипт в котором будет характеристики лук, меч, нож и их очки нанесения урона а также хп игрока, получение игроком урона

    public int hp;
    public bool isBattle;
    public List<GameObject> allEnemies;

    void Start()
    {
        allEnemies = new List<GameObject>();
    }

    void Update()
    {
        if (allEnemies.Count != 0)
        {
            isBattle = true;
        }
        else
        {
            isBattle = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "SwordEn")
        {
            hp -= Random.Range(30, 70);
        }
        else if (other.gameObject.tag == "KnifeEn")
        {
            hp -= Random.Range(10, 30);
        }
        else if (other.gameObject.tag == "ArrowEn")
        {
            hp -= Random.Range(30, 350);
        }
    }
}