using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemyes : MonoBehaviour
{
    public Dictionary<string, int> allEnemies;
    private Dictionary<string, int> _allEnemiesCount;
    public GameObject allySoldier;
    public GameObject partisans;
    public GameObject enemySoldier;
    public Transform[] spawnEnemy;
    private bool _init;

    public GameObject arrow;

    private void Awake()
    {
        allEnemies = new Dictionary<string, int>();
        _allEnemiesCount = new Dictionary<string, int>();
        allEnemies.Add("AllySoldier", 0);
        allEnemies.Add("Partisans", 0);
        allEnemies.Add("EnemySoldier", 0);
        _init = true;
    }

    void Update()
    {
        if (_init)
        {
            Initialize();
            StartCoroutine(CheckEnemies());
        }
    }

    private IEnumerator CheckEnemies()
    {
        while (true)
        {
            float _needEnCount = Random.Range(0.6f, 0.9f);
            if (allEnemies["AllySoldier"] < _allEnemiesCount["AllySoldier"] * _needEnCount)
            {
                GameObject enemy = Instantiate(allySoldier, spawnEnemy[Random.Range(0, spawnEnemy.Length)].position, Quaternion.identity);
            }
            if (allEnemies["Partisans"] < _allEnemiesCount["Partisans"] * _needEnCount)
            {
                GameObject enemy = Instantiate(partisans, spawnEnemy[Random.Range(0, spawnEnemy.Length)].position, Quaternion.identity);
            }
            if (allEnemies["EnemySoldier"] < _allEnemiesCount["EnemySoldier"] * _needEnCount)
            {
                GameObject enemy = Instantiate(enemySoldier, spawnEnemy[Random.Range(0, spawnEnemy.Length)].position, Quaternion.identity);
            }
            yield return new WaitForSeconds(30.0f);
        }
    }

    private void Initialize()
    {
        _init = false;
        foreach (var item in allEnemies)
        {
            _allEnemiesCount.Add(item.Key, item.Value);
        }
    }
}