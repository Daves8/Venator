using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FinalBattle : MonoBehaviour
{
    private GameObject[] allySoldiers;
    private GameObject[] enemySoldiers;


    public GameObject firstBattleMain;
    public GameObject firstBattle1;
    public GameObject firstBattle2;
    public GameObject mainBattle;

    private ImportantBuildings _importantBuildings;

    private bool _battle1 = false;
    private bool _battle2 = false;
    private bool _battleFirstMain = false;

    private int[] index = { 5, 4, 8, 2, 3, 7, 0, 9, 1, 6 };
    private int indx = 1;

    void Start()
    {
        allySoldiers = GameObject.FindGameObjectsWithTag("Enemy");
        enemySoldiers = GameObject.FindGameObjectsWithTag("EnemySoldier");

        _importantBuildings = GameObject.FindGameObjectWithTag("BuildingsImportant").GetComponent<ImportantBuildings>();
    }

    void Update()
    {






        if (!_battle1)
        {
            Battle1();
        }
        if (!_battleFirstMain)
        {
            FirstBattleMain();
        }
        if (!_battle2)
        {
            Battle2();
        }









        /*
        allySoldiers[0].GetComponent<Enemy>()._agressive = true;
        allySoldiers[0].GetComponent<Enemy>()._player = allySoldiers[5];

        allySoldiers[1].GetComponent<Enemy>()._agressive = true;
        allySoldiers[1].GetComponent<Enemy>()._player = allySoldiers[6];

        allySoldiers[2].GetComponent<Enemy>()._agressive = true;
        allySoldiers[2].GetComponent<Enemy>()._player = allySoldiers[7];

        allySoldiers[3].GetComponent<Enemy>()._agressive = true;
        allySoldiers[3].GetComponent<Enemy>()._player = allySoldiers[8];

        allySoldiers[4].GetComponent<Enemy>()._agressive = true;
        allySoldiers[4].GetComponent<Enemy>()._player = allySoldiers[9];




        allySoldiers[5].GetComponent<Enemy>()._agressive = true;
        allySoldiers[5].GetComponent<Enemy>()._player = allySoldiers[1];

        allySoldiers[6].GetComponent<Enemy>()._agressive = true;
        allySoldiers[6].GetComponent<Enemy>()._player = allySoldiers[2];

        allySoldiers[7].GetComponent<Enemy>()._agressive = true;
        allySoldiers[7].GetComponent<Enemy>()._player = allySoldiers[3];

        allySoldiers[8].GetComponent<Enemy>()._agressive = true;
        allySoldiers[8].GetComponent<Enemy>()._player = allySoldiers[4];

        allySoldiers[9].GetComponent<Enemy>()._agressive = true;
        allySoldiers[9].GetComponent<Enemy>()._player = allySoldiers[5];
        */
    }

    public void GoToBattlePoints()
    {
        for (int i = 0; i < 6; i++) // 1
        {
            allySoldiers[i].GetComponent<NavMeshAgent>().SetDestination(firstBattle1.transform.position + new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f)));

            enemySoldiers[i].GetComponent<NavMeshAgent>().enabled = false;
            enemySoldiers[i].transform.position = firstBattle1.transform.position + new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f));
            enemySoldiers[i].transform.rotation = Quaternion.Euler(0f, Random.Range(0, 360), 0f);
            enemySoldiers[i].GetComponent<NavMeshAgent>().enabled = true;
        }
        for (int i = 6; i < 14; i++) // 2
        {
            allySoldiers[i].GetComponent<NavMeshAgent>().SetDestination(firstBattleMain.transform.position + new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f)));

            enemySoldiers[i].GetComponent<NavMeshAgent>().enabled = false;
            enemySoldiers[i].transform.position = firstBattleMain.transform.position + new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f));
            enemySoldiers[i].transform.rotation = Quaternion.Euler(0f, Random.Range(0, 360), 0f);
            enemySoldiers[i].GetComponent<NavMeshAgent>().enabled = true;
        }
        for (int i = 14; i < 20; i++) // first main
        {
            allySoldiers[i].GetComponent<NavMeshAgent>().SetDestination(firstBattle2.transform.position + new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f)));

            enemySoldiers[i].GetComponent<NavMeshAgent>().enabled = false;
            enemySoldiers[i].transform.position = firstBattle2.transform.position + new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f));
            enemySoldiers[i].transform.rotation = Quaternion.Euler(0f, Random.Range(0, 360), 0f);
            enemySoldiers[i].GetComponent<NavMeshAgent>().enabled = true;
        }

        for (int i = 20; i < 27; i++) // first main
        {
            enemySoldiers[i].GetComponent<NavMeshAgent>().enabled = false;
            enemySoldiers[i].transform.position = _importantBuildings.EntranceToTavern.transform.position + new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f));
            enemySoldiers[i].transform.rotation = Quaternion.Euler(0f, Random.Range(0, 360), 0f);
            enemySoldiers[i].GetComponent<NavMeshAgent>().enabled = true;
        }




        /*
        // противники - телепорт
        for (int i = 10; i < 14; i++) // 1
        {
            enemySoldiers[i].GetComponent<NavMeshAgent>().enabled = false;
            enemySoldiers[i].transform.position = firstBattle1.transform.position + new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f));
            enemySoldiers[i].transform.rotation = Quaternion.Euler(0f, Random.Range(0, 360), 0f);
            enemySoldiers[i].GetComponent<NavMeshAgent>().enabled = true;
        }
        for (int i = 14; i < 19; i++) // 2
        {
            enemySoldiers[i].GetComponent<NavMeshAgent>().enabled = false;
            enemySoldiers[i].transform.position = firstBattleMain.transform.position + new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f));
            enemySoldiers[i].transform.rotation = Quaternion.Euler(0f, Random.Range(0, 360), 0f);
            enemySoldiers[i].GetComponent<NavMeshAgent>().enabled = true;
        }
        for (int i = 19; i < 24; i++) // first main
        {
            enemySoldiers[i].GetComponent<NavMeshAgent>().enabled = false;
            enemySoldiers[i].transform.position = firstBattle2.transform.position + new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f));
            enemySoldiers[i].transform.rotation = Quaternion.Euler(0f, Random.Range(0, 360), 0f);
            enemySoldiers[i].GetComponent<NavMeshAgent>().enabled = true;
        }
        */

        // спавн на точки вроажеских солдат  и послать в эти же точки союзников
        // когда союзники подойдут на 5 метров к точке, напасть союзниками на вражеских солдат
        // битва длится некоторое время
        // дали буде
    }

    public void Battle1()
    {
        if (Vector3.Distance(allySoldiers[0].transform.position, enemySoldiers[0].transform.position) <= 25f)
        {
            _battle1 = true;
            for (int i = 0; i < 6; i++) // 1
            {
                allySoldiers[i].GetComponent<Enemy>()._agressive = true;
                allySoldiers[i].GetComponent<Enemy>()._player = enemySoldiers[i];

                enemySoldiers[i].GetComponent<Enemy>()._agressive = true;
                enemySoldiers[i].GetComponent<Enemy>()._player = allySoldiers[i];
            }
        }
    }

    public void FirstBattleMain()
    {
        if (Vector3.Distance(allySoldiers[6].transform.position, enemySoldiers[6].transform.position) <= 25f)
        {
            _battleFirstMain = true;
            for (int i = 6; i < 14; i++) // firstMain
            {
                allySoldiers[i].GetComponent<Enemy>()._agressive = true;
                allySoldiers[i].GetComponent<Enemy>()._player = enemySoldiers[i];

                enemySoldiers[i].GetComponent<Enemy>()._agressive = true;
                enemySoldiers[i].GetComponent<Enemy>()._player = allySoldiers[i];
            }
        }
    }

    public void Battle2()
    {
        if (Vector3.Distance(allySoldiers[14].transform.position, enemySoldiers[14].transform.position) <= 25f)
        {
            _battle2 = true;
            for (int i = 14; i < 20; i++) // 2
            {
                allySoldiers[i].GetComponent<Enemy>()._agressive = true;
                allySoldiers[i].GetComponent<Enemy>()._player = enemySoldiers[i];

                enemySoldiers[i].GetComponent<Enemy>()._agressive = true;
                enemySoldiers[i].GetComponent<Enemy>()._player = allySoldiers[i];
            }
        }
    }

    public void KillSomeEnemy()
    {
        enemySoldiers[indx].GetComponent<Enemy>()._hp = -100;
        //enemySoldiers[indx] = enemySoldiers[0];
        if (indx % 2 == 0)
        {
            allySoldiers[indx].GetComponent<Enemy>()._hp = -100;
            //allySoldiers[indx] = allySoldiers[0];
        }
        indx++;
    }
    public void KillSomeAlly()
    {
        int index = Random.Range(0, allySoldiers.Length);
    }
    public void Final()
    {
        int indx = 20;
        for (int i = 0; i < 20; i++) // 2
        {
            if (i % 2 != 0)
            {
                if (indx == 27)
                {
                    indx = 20;
                }
                //allySoldiers[i].GetComponent<NavMeshAgent>().SetDestination(_importantBuildings.EntranceToTavern.transform.position + new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f)));

                allySoldiers[i].GetComponent<Enemy>()._agressive = true;
                allySoldiers[i].GetComponent<Enemy>()._player = enemySoldiers[indx];

                enemySoldiers[indx].GetComponent<Enemy>()._agressive = true;
                enemySoldiers[indx].GetComponent<Enemy>()._player = allySoldiers[i];
                indx++;
            }
        }

    }
}