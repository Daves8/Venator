using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacteristics : MonoBehaviour
{
    public int hp;
    private bool _dead;
    public bool isBattle;
    public bool isBattleAnimal;
    public bool crouch;
    public List<GameObject> allEnemies;
    public List<GameObject> allAnimals;
    public Place place;
    public GameObject sword;
    public float damageSword;
    public GameObject[] allSwords; // 0 - базовый меч, 1 - продвинутый меч и т.д.

    public GameObject knife;
    public float damageKnife;
    public GameObject[] allKnifes; // 0 - базовый нож, 1 - продвинутый нож и т.д.

    private CharacterMoving _chMove;

    void Start()
    {
        allEnemies = new List<GameObject>();
        _chMove = GetComponent<CharacterMoving>();
        hp = 500;
        _dead = false;
        damageSword = 50;
        damageKnife = 20;
    }

    void Update()
    {
        if (_dead)
        {
            return;
        }
        if (hp <= 0)
        {
            Death();
            return;
        }

        if (allEnemies.Count != 0)
        {
            isBattle = true;
        }
        else
        {
            isBattle = false;
        }
        if (allAnimals.Count != 0)
        {
            isBattleAnimal = true;
        }
        else
        {
            isBattleAnimal = false;
        }

        crouch = _chMove._isCrouch;

        Teleport();
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
            hp -= Random.Range(30, 100);
        }
    }
    private void Death()
    {
        _dead = true;
        print("Умер!");
    }
    private void Teleport()
    {
        if (Input.GetKeyDown(KeyCode.Backslash))
        {
            if (place == Place.village)
            {
                // в лес
                GetComponent<CharacterController>().enabled = false;
                transform.position = new Vector3(282f, -166f, -2180f);
                GetComponent<CharacterController>().enabled = true;
            }
            else if (place == Place.forest)
            {
                // в деревню
                GetComponent<CharacterController>().enabled = false;
                transform.position = new Vector3(915f, 3.8f, 673f);
                GetComponent<CharacterController>().enabled = true;
            }
        }
        //else if (Input.GetKeyDown(KeyCode.Less))
        //{
        //    // в деревню
        //    GetComponent<CharacterController>().enabled = false;
        //    transform.position = new Vector3(915f, 3.8f, 673f);
        //    GetComponent<CharacterController>().enabled = true;
        //}
    }

    public enum Place
    {
        village,
        forest
    }
}