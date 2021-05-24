using System.Data.Common;
using System.Runtime.ExceptionServices;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public InventoryObject inventory;
    public InventoryObject equipment;
    public bool test = false;

    public Transform playerPosition;
    public Animator transition;
//PLayerData
    public string level;    
//PLayerData

    public Attribute[] attributes;

    private Transform boots;
    private Transform chest;
    private Transform helmet;
    private Transform offhand;
    private Transform sword;

    private CharacterController _controller;
    public Transform weaponTransform;
    public Transform offhandWristTransform;
    public Transform offhandHandTransform;

    public BoneCombiner boneCombiner;
    private ItemButton itemButton;
    private ScrollView scrollView;    
    

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        scrollView = new ScrollView();
        
        if(SceneManager.GetActiveScene().name == "Village")
        {
            string loadPath = DataHolder.savePath;
            Debug.Log("Сейчас мы загружаем уровень: " + loadPath);
            LoadPlayer(loadPath);
            
        }
        
    }

    private void Start()
    {    
        for (int i = 0; i < attributes.Length; i++)
        {
            attributes[i].SetParent(this);
        }

        for (int i = 0; i < equipment.GetSlots.Length; i++)
        {
            equipment.GetSlots[i].OnBeforeUpdate += OnRemoveItem;
            equipment.GetSlots[i].OnAfterUpdate += OnAddItem;
        }   
    }

    public void StartBlackScreen()
    {
        transition.SetTrigger("Start");
    }

    public void EndBlackScreen()
    {
        transition.SetTrigger("End");
    }

    public int SearchInInventary(int id)
    {
        int count = 0;
        // поиск по всему инвентарю циклом
        count  = inventory.FindItemOnInventory(id);
        //
        return count;
    }

    public void MakeLoadPath(ItemButton saveButton)
    {
        DataHolder.savePath = saveButton.savePath;
    }

    public void MakeNewGamePath()
    {
        DataHolder.savePath = Application.persistentDataPath + "/Saves/"+ "NewGame" +".bin";
        SaveSystem.SavePlayer();
    }

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayer(string savePath)
    {
    
        //Перезаписываем файл х текущими значениями сцены
        //загружаем сцену
        //Debug.Log(test);--
        test = true;
        //Invoke("StartBlackScreen", 0.5f);
        PlayerData data = SaveSystem.LoadPlayer(savePath);

        //level = data.level;

        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];

        _controller.enabled = false;
        //playerPosition.position = new Vector3(0,200,0);
        playerPosition.position = position;
        _controller.enabled = true;

        // инвентарь
        //inventory = data.inventory;
        //equipment = data.equipment;

       
        //Invoke("EndBlackScreen", 1f);
        
        
    }

    public void OnRemoveItem(InventorySlot _slot)
    {
        if (_slot.ItemObject == null)
            return;
        switch (_slot.parent.inventory.type)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                print(string.Concat("Removed ", _slot.ItemObject, " on ", _slot.parent.inventory.type,
                    ", Allowed Items: ", string.Join(", ", _slot.AllowedItems)));

                for (int i = 0; i < _slot.item.buffs.Length; i++)
                {
                    for (int j = 0; j < attributes.Length; j++)
                    {
                        if (attributes[j].type == _slot.item.buffs[i].attribute)
                            attributes[j].value.RemoveModifier(_slot.item.buffs[i]);
                    }
                }

                if (_slot.ItemObject.characterDisplay != null)
                {
                    switch (_slot.AllowedItems[0])
                    {
                        case ItemType.Helmet:
                            Destroy(helmet.gameObject);
                            break;
                        case ItemType.Weapon:
                            Destroy(sword.gameObject);
                            break;
                        case ItemType.Shield:
                            Destroy(offhand.gameObject);
                            break;
                        case ItemType.Boots:
                            Destroy(boots.gameObject);
                            break;
                        case ItemType.Chest:
                            try{Destroy(chest.gameObject);}
                            catch{}
                            break;
                    }
                }
                break;
            case InterfaceType.Chest:
                break;
            default:
                break;
        }
    }

    public void OnAddItem(InventorySlot _slot)
    {        
        if (_slot.ItemObject == null)
        {
            //Debug.Log("СПППП");
            return;
        }else
        {
            Debug.Log(_slot.parent.inventory.type);
            switch (_slot.parent.inventory.type)
            {
                case InterfaceType.Inventory:
                    break;
                case InterfaceType.Equipment:
                    print(
                        $"Placed {_slot.ItemObject}  on {_slot.parent.inventory.type}, Allowed Items: {string.Join(", ", _slot.AllowedItems)}");

                    for (int i = 0; i < _slot.item.buffs.Length; i++)
                    {
                        for (int j = 0; j < attributes.Length; j++)
                        {
                            if (attributes[j].type == _slot.item.buffs[i].attribute)
                                attributes[j].value.AddModifier(_slot.item.buffs[i]);
                        }
                    }
                    Debug.Log(_slot.ItemObject.equipmentSetId);
                        Debug.Log("тут чет есть");
                        switch (_slot.AllowedItems[0])
                        {
                            case ItemType.Helmet://
                                    ChangeClothes.Change(_slot.ItemObject.equipmentSetId);                                                                                  
                                break;
                            case ItemType.Weapon:                        
                                sword = Instantiate(_slot.ItemObject.characterDisplay, weaponTransform).transform;
                                break;
                            case ItemType.Shield:                        
                                switch (_slot.ItemObject.type)
                                {
                                    case ItemType.Weapon:
                                        offhand = Instantiate(_slot.ItemObject.characterDisplay, offhandHandTransform)
                                            .transform;
                                        break;
                                    case ItemType.Shield:
                                        offhand = Instantiate(_slot.ItemObject.characterDisplay, offhandWristTransform)
                                            .transform;
                                        break;
                                }
                                break;
                        } 
                    break;
                case InterfaceType.Chest:
                    break;
                default:
                {
                    Debug.Log("DOnt work");
                    break;
                }
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        /*var groundItem = other.GetComponent<GroundItem>();
        if (groundItem)
        {
            Item _item = new Item(groundItem.item);
            if (inventory.AddItem(_item, 1))
            {
                Destroy(other.gameObject);
            }
        }*/
    }

    private void Update()
    {
        //Debug.Log(playerPosition.position);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            inventory.Save();
            equipment.Save();
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            inventory.Load();
            equipment.Load();
        }
    }

    public void AttributeModified(Attribute attribute)
    {
        Debug.Log(string.Concat(attribute.type, " was updated! Value is now ", attribute.value.ModifiedValue));
    }

    private void OnApplicationQuit()
    {
        inventory.Clear();
        equipment.Clear();
    }
}

[System.Serializable]
public class Attribute
{
    [System.NonSerialized] public Player parent;
    public Attributes type;
    public ModifiableInt value;

    public void SetParent(Player _parent)
    {
        parent = _parent;
        value = new ModifiableInt(AttributeModified);
    }

    public void AttributeModified()
    {
        parent.AttributeModified(this);
    }
}