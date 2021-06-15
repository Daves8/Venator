using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacteristics : MonoBehaviour
{
    public int hp;
    public int maxHp;
    public bool _dead;
    private bool _deadHelper = true;
    public bool isBattle;
    public bool isBattleAnimal;
    public bool crouch;
    public List<GameObject> allEnemies;
    public List<GameObject> allAnimals;
    public Place place;
    public GameObject sword;
    public float damageSword;
    public GameObject[] allSwords; // 0 - базовый меч, 1 - продвинутый меч и т.д. // кароче тут префабы мечей

    public GameObject[] allSwordsOn; // а тут - мечи уже с правильным расположением
    public GameObject[] allSwordsOff; //


    public GameObject knife;
    public float damageKnife;
    public GameObject[] allKnifes; // 0 - базовый нож, 1 - продвинутый нож и т.д.

    public GameObject bow; // лук
    public GameObject[] allBows;

    public GameObject[] allBowsOn;
    public GameObject[] allBowsOff;
    public GameObject quiver;

    private CharacterMoving _chMove;
    private Battle _battleScripts;

    public GameObject DeathUI;
    private bool DeathUIOnOff = false;

    void Start()
    {
        allEnemies = new List<GameObject>();
        _chMove = GetComponent<CharacterMoving>();
        hp = 500;
        maxHp = 500;
        _dead = false;
        damageSword = 200;
        damageKnife = 100;
        _battleScripts = GetComponent<Battle>();
    }

    public void showHideDeath()
    {
        DeathUIOnOff = !DeathUIOnOff;
        DeathUI.SetActive(DeathUIOnOff);
        if (DeathUIOnOff)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
        }
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
        if (hp > maxHp)
        {
            hp = maxHp;
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
        ChangeWeapons();
    }

    private void ChangeWeapons()
    {
        //------------------------------------------ в зависимости от того какой у нас меч, мы меняем его для Battle
        if (sword == allSwords[0])
        {
            if (_battleScripts._swordOn != allSwordsOn[0])
            {
                ////////////////////////////////////////////////////////////////////////////////////////////
            }
            _battleScripts._swordOn = allSwordsOn[0];
            _battleScripts._swordOff = allSwordsOff[0];
            allSwordsOn[1].SetActive(false);
            allSwordsOff[1].SetActive(false);
        }
        else if (sword == allSwords[1])
        {
            if (_battleScripts._swordOn != allSwordsOn[1])
            {
                ////////////////////////////////////////////////////////////////////////////////
            }
            _battleScripts._swordOn = allSwordsOn[1];
            _battleScripts._swordOff = allSwordsOff[1];
            allSwordsOn[0].SetActive(false);
            allSwordsOff[0].SetActive(false);
        }
        else
        {
            allSwordsOn[1].SetActive(false);
            allSwordsOff[1].SetActive(false);
            allSwordsOn[0].SetActive(false);
            allSwordsOff[0].SetActive(false);
        }

        if (bow == allBows[0])
        {
            if (_battleScripts._bowOn != allBowsOn[0])
            {
                ///////////////////////////////////////////////////////////////////////////
            }
            _battleScripts._bowOn = allBowsOn[0];
            _battleScripts._bowOff = allBowsOff[0];
            quiver.SetActive(true);
        }
        else
        {
            allBowsOn[0].SetActive(false);
            allBowsOff[0].SetActive(false);
            quiver.SetActive(false);
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
            hp -= Random.Range(30, 100);
        }

        // животные
        if (other.gameObject.tag == "DamageBear")
        {
            hp -= Random.Range(300, 400);
        }
        else if (other.gameObject.tag == "DamageBoar")
        {
            hp -= Random.Range(150, 200);
        }
        else if (other.gameObject.tag == "DamageWolf")
        {
            hp -= Random.Range(150, 250);
        }
    }
    private void Death()
    {
        _dead = true;
        allEnemies.Clear();
        allAnimals.Clear();
        isBattle = false;
        isBattleAnimal = false;
        int timeToUI = Random.Range(1, 3);
        Invoke("showHideDeath", 2.0f);
        GetComponent<Animator>().SetTrigger("Death" + timeToUI);
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