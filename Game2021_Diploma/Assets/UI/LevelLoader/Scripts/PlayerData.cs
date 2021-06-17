using System.Security.AccessControl;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData 
{
    public string level;
    public float[] position;
    //public InventoryObject inventory;
    //public InventoryObject equipment;
    public Inventory inventory;
    public Inventory equipment;
    public Quest quest;
    public int[] resultQuests;
    public int resultGame;
    public int gold;
    public bool attackOnPop;
    public int subquest;

    public PlayerData(Player player)//if continue
    {
        level = "LoadedLevel";//Можно удалить (PlayerData.cs, Player.cs)

        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;

        //инвентарь
        inventory = player.GetComponent<Player>().inventory.Container;
        equipment = player.GetComponent<Player>().equipment.Container;
        //квесты
        gold = player.gameObject.GetComponent<PlayerCharacteristics>().gold;
        attackOnPop = player.gameObject.GetComponent<PlayerCharacteristics>().attackOnPopulation;
        quest = GameObject.FindGameObjectWithTag("QuestsManag").GetComponent<QuestsManagement>().quest;
        resultQuests = GameObject.FindGameObjectWithTag("QuestsManag").GetComponent<QuestsManagement>().resultQuests;
        resultGame = GameObject.FindGameObjectWithTag("QuestsManag").GetComponent<QuestsManagement>().resultGame;
        switch (GameObject.FindGameObjectWithTag("QuestsManag").GetComponent<QuestsManagement>().quest)
        {
            case Quest.none:
                break;
            case Quest.quest1:
                subquest = (int)GameObject.FindGameObjectWithTag("QuestsManag").GetComponent<Quest1>().subquest;
                break;
            case Quest.quest2:
                subquest = (int)GameObject.FindGameObjectWithTag("QuestsManag").GetComponent<Quest2>().subquest;
                break;
            case Quest.quest3:
                subquest = (int)GameObject.FindGameObjectWithTag("QuestsManag").GetComponent<Quest3>().subquest;
                break;
            case Quest.quest4:
                subquest = (int)GameObject.FindGameObjectWithTag("QuestsManag").GetComponent<Quest4>().subquest;
                break;
            default:
                break;
        }
    }

    public PlayerData()//if newgame
    {
        level = "NewGame";

        position = new float[3];
        position[0] = 978.68f;
        position[1] = 0;
        position[2] = 1165.35f;

        //inventory.Clear();
        //equipment.Clear();
        //квесты
        
        gold = 0;
        attackOnPop = false;
        quest = Quest.quest1;
        resultQuests = new int[4];
        resultGame = 0;
        subquest = (int)Quest1.Subquest.subquest1;
        /*
        switch (GameObject.FindGameObjectWithTag("QuestsManag").GetComponent<QuestsManagement>().quest)
        {
            case Quest.none:
                break;
            case Quest.quest1:
                subquest = (int)GameObject.FindGameObjectWithTag("QuestsManag").GetComponent<Quest1>().subquest;
                break;
            case Quest.quest2:
                subquest = (int)GameObject.FindGameObjectWithTag("QuestsManag").GetComponent<Quest2>().subquest;
                break;
            case Quest.quest3:
                subquest = (int)GameObject.FindGameObjectWithTag("QuestsManag").GetComponent<Quest3>().subquest;
                break;
            case Quest.quest4:
                subquest = (int)GameObject.FindGameObjectWithTag("QuestsManag").GetComponent<Quest4>().subquest;
                break;
            default:
                break;
        }
        */

        //ошибка с инвентарем, нельзя получить доступ к нему
        // квесты
        // инвернтарь
    }
}
