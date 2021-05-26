using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalLimbs : MonoBehaviour
{
    public GameObject parent;
    public ParentAnimal typeParent;
    private PlayerCharacteristics _playerCharacteristics;

    private void Start()
    {
        _playerCharacteristics = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacteristics>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Arrow")
        {
            float damage = Random.Range(30, 70);
            switch (typeParent)
            {
                case ParentAnimal.Wolf:
                    Wolf wolf = parent.GetComponent<Wolf>();
                    wolf.Agressive();
                    wolf.hp -= damage;
                    break;
                case ParentAnimal.Bear:
                    Bear bear = parent.GetComponent<Bear>();
                    bear.Agressive();
                    bear.hp -= damage;
                    break;
                case ParentAnimal.ForestAnimal:
                    ForestAnimal forestAnimal = parent.GetComponent<ForestAnimal>();
                    forestAnimal.Agressive();
                    forestAnimal.hp -= damage;
                    break;
                case ParentAnimal.VillageAnimal:
                    VillageAnimal villagetAnimal = parent.GetComponent<VillageAnimal>();
                    villagetAnimal.Agressive();
                    villagetAnimal.hp -= damage;
                    break;
                default:
                    break;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // охотник
        if (other.gameObject.tag == "SwordEn")
        {
            float damage = Random.Range(30, 70);
            switch (typeParent)
            {
                case ParentAnimal.Wolf:
                    Wolf wolf = parent.GetComponent<Wolf>();
                    wolf.Agressive();
                    wolf.hp -= damage;
                    break;
                case ParentAnimal.Bear:
                    Bear bear = parent.GetComponent<Bear>();
                    bear.Agressive();
                    bear.hp -= damage;
                    break;
                case ParentAnimal.ForestAnimal:
                    ForestAnimal forestAnimal = parent.GetComponent<ForestAnimal>();
                    forestAnimal.Agressive();
                    forestAnimal.hp -= damage;
                    break;
                case ParentAnimal.VillageAnimal:
                    VillageAnimal villagetAnimal = parent.GetComponent<VillageAnimal>();
                    villagetAnimal.Agressive();
                    villagetAnimal.hp -= damage;
                    break;
                default:
                    break;
            }
        }
        else if (other.gameObject.tag == "KnifeEn")
        {
            float damage = Random.Range(10, 30);
            switch (typeParent)
            {
                case ParentAnimal.Wolf:
                    Wolf wolf = parent.GetComponent<Wolf>();
                    wolf.Agressive();
                    wolf.hp -= damage;
                    break;
                case ParentAnimal.Bear:
                    Bear bear = parent.GetComponent<Bear>();
                    bear.Agressive();
                    bear.hp -= damage;
                    break;
                case ParentAnimal.ForestAnimal:
                    ForestAnimal forestAnimal = parent.GetComponent<ForestAnimal>();
                    forestAnimal.Agressive();
                    forestAnimal.hp -= damage;
                    break;
                case ParentAnimal.VillageAnimal:
                    VillageAnimal villagetAnimal = parent.GetComponent<VillageAnimal>();
                    villagetAnimal.Agressive();
                    villagetAnimal.hp -= damage;
                    break;
                default:
                    break;
            }
        }
        // игрок
        if (other.gameObject.tag == "Sword") // 100-150 меч 2-го уровня
        {
            float damage = Random.Range(_playerCharacteristics.damageSword * 0.75f, _playerCharacteristics.damageSword * 1.25f);
            switch (typeParent)
            {
                case ParentAnimal.Wolf:
                    Wolf wolf = parent.GetComponent<Wolf>();
                    wolf.Agressive();
                    wolf.hp -= damage;
                    break;
                case ParentAnimal.Bear:
                    Bear bear = parent.GetComponent<Bear>();
                    bear.Agressive();
                    bear.hp -= damage;
                    break;
                case ParentAnimal.ForestAnimal:
                    ForestAnimal forestAnimal = parent.GetComponent<ForestAnimal>();
                    forestAnimal.Agressive();
                    forestAnimal.hp -= damage;
                    break;
                case ParentAnimal.VillageAnimal:
                    VillageAnimal villagetAnimal = parent.GetComponent<VillageAnimal>();
                    villagetAnimal.Agressive();
                    villagetAnimal.hp -= damage;
                    break;
                default:
                    break;
            }
        }
        else if (other.gameObject.tag == "Knife")
        {
            float damage = Random.Range(_playerCharacteristics.damageKnife * 0.75f, _playerCharacteristics.damageKnife * 1.25f);
            switch (typeParent)
            {
                case ParentAnimal.Wolf:
                    Wolf wolf = parent.GetComponent<Wolf>();
                    wolf.Agressive();
                    wolf.hp -= damage;
                    break;
                case ParentAnimal.Bear:
                    Bear bear = parent.GetComponent<Bear>();
                    bear.Agressive();
                    bear.hp -= damage;
                    break;
                case ParentAnimal.ForestAnimal:
                    ForestAnimal forestAnimal = parent.GetComponent<ForestAnimal>();
                    forestAnimal.Agressive();
                    forestAnimal.hp -= damage;
                    break;
                case ParentAnimal.VillageAnimal:
                    VillageAnimal villagetAnimal = parent.GetComponent<VillageAnimal>();
                    villagetAnimal.Agressive();
                    villagetAnimal.hp -= damage;
                    break;
                default:
                    break;
            }
        }
    }
    public enum ParentAnimal
    {
        Wolf,
        Bear,
        ForestAnimal,
        VillageAnimal
    }
}