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
    }

    public PlayerData()//if newgame
    {
        level = "NewGame";

        position = new float[3];
        position[0] = 0;
        position[1] = -4;
        position[2] = 0;

        //ошибка с инвентарем, нельзя получить доступ к нему
        // квесты
        // инвернтарь
    }
}
