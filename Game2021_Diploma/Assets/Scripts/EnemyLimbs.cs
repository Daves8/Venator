using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLimbs : MonoBehaviour
{
    public GameObject parentEnemy;
    public TypeEnemy type;
    private PlayerCharacteristics _playerCharact;

    void Start()
    {
        _playerCharact = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacteristics>();
    }

    private void OnCollisionEnter(Collision other) // для стрел
    {
        if (other.gameObject.tag == "Arrow")
        {
            switch (type)
            {
                case TypeEnemy.enemy:
                    Add(parentEnemy);
                    Enemy enemy = parentEnemy.GetComponent<Enemy>();
                    enemy._agressive = true;
                    enemy._hp -= Random.Range(30, 100);
                    break;
                case TypeEnemy.hunter:
                    Add(parentEnemy);
                    Hunter hunter = parentEnemy.GetComponent<Hunter>();
                    //hunter._agressive = true;
                    hunter.hp -= Random.Range(30, 100);
                    break;
                case TypeEnemy.soldier:
                    Add(parentEnemy);
                    Soldier soldier = parentEnemy.GetComponent<Soldier>();
                    //soldier._agressive = true;
                    soldier.hp -= Random.Range(30, 100);
                    break;
                default:
                    break;
            }

        }
    }
    private void OnTriggerEnter(Collider other) // для оружия ближнего боя
    {

        if (other.gameObject.tag == "Sword")
        {
            float damage = Random.Range(_playerCharact.damageSword * 0.75f, _playerCharact.damageSword * 1.25f);
            switch (type)
            {
                case TypeEnemy.enemy:
                    Enemy enemy = parentEnemy.GetComponent<Enemy>();
                    enemy._agressive = true;
                    Add(parentEnemy);
                    enemy._hp -= damage;
                    break;
                case TypeEnemy.hunter:
                    Hunter hunter = parentEnemy.GetComponent<Hunter>();
                    //hunter._agressive = true;
                    Add(parentEnemy);
                    hunter.hp -= damage;
                    break;
                case TypeEnemy.soldier:
                    Soldier soldier = parentEnemy.GetComponent<Soldier>();
                    //hunter._agressive = true;
                    Add(parentEnemy);
                    soldier.hp -= damage;
                    break;
                default:
                    break;
            }
        }
        else if (other.gameObject.tag == "Knife")
        {
            float damage = Random.Range(_playerCharact.damageKnife * 0.75f, _playerCharact.damageKnife * 1.25f);
            switch (type)
            {
                case TypeEnemy.enemy:
                    Enemy enemy = parentEnemy.GetComponent<Enemy>();
                    enemy._agressive = true;
                    Add(parentEnemy);
                    enemy._hp -= damage;
                    break;
                case TypeEnemy.hunter:
                    Hunter hunter = parentEnemy.GetComponent<Hunter>();
                    //hunter._agressive = true;
                    Add(parentEnemy);
                    hunter.hp -= damage;
                    break;
                case TypeEnemy.soldier:
                    Soldier soldier = parentEnemy.GetComponent<Soldier>();
                    //soldier._agressive = true;
                    Add(parentEnemy);
                    soldier.hp -= damage;
                    break;
                default:
                    break;
            }

        }
    }
    private void Add(GameObject enemy)
    {
        if (!_playerCharact.allEnemies.Contains(enemy))
        {
            _playerCharact.allEnemies.Add(enemy);
        }
    }

    public enum TypeEnemy
    {
        enemy,
        hunter,
        soldier
    }
}