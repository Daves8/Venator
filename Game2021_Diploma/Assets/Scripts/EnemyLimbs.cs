using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLimbs : MonoBehaviour
{
    public GameObject parentEnemy;
    private Enemy _enemy;
    private PlayerCharacteristics _playerCharact;

    void Start()
    {
        _enemy = parentEnemy.GetComponent<Enemy>();
        _playerCharact = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacteristics>();
    }

    private void OnCollisionEnter(Collision other) // для стрел
    {
        if (!_enemy._death)
        {
            if (other.gameObject.tag == "Arrow")
            {
                Add(parentEnemy);
                _enemy._agressive = true;
                _enemy._hp -= Random.Range(30, 100);
            }
        }
    }
    private void OnTriggerEnter(Collider other) // для оружия ближнего боя
    {
        if (!_enemy._death)
        {
            if (other.gameObject.tag == "Sword")
            {
                float damage = Random.Range(_playerCharact.damageSword * 0.75f, _playerCharact.damageSword * 1.25f);
                _enemy._agressive = true;
                Add(parentEnemy);
                _enemy._hp -= damage;
            }
            else if (other.gameObject.tag == "Knife")
            {
                float damage = Random.Range(_playerCharact.damageKnife * 0.75f, _playerCharact.damageKnife * 1.25f);
                _enemy._agressive = true;
                Add(parentEnemy);
                _enemy._hp -= damage;
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
}