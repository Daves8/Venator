using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fishing : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _river;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _text;

    public InventoryObject inventory;
    public ItemObject fish;

    private bool _readyToFishing = false;
    private bool _startFishing = false;
    public bool NowFishing = false;

    private bool _nextDo = false;

    private int _count = 0;

    private void Update()
    {
        if (_readyToFishing && Input.GetButtonDown("Action"))
        {
            CharacterMoving.IsReadyToMove = false;
            _player.GetComponent<Battle>().AllowBattle = false;
            //_readyToFishing = false;
            NowFishing = true;

            _animator.SetTrigger("StartFishing");
            // поворот player в сторону реки (поворот по оси Y)

            Invoke("GetFish", Random.Range(4, 16));
            _count = 0;
        }

        //if (true /*какое-то действие, которое означает конец рыбалки*/&& Input.GetButtonDown("Jump"))
        //{
        //    _animator.SetTrigger("Idle");
        //    CharacterMoving.IsReadyToMove = true;
        //    Battle.AllowBattle = true;
        //    _readyToFishing = false;
        //    _fishing = false;

        //    //_animator.SetFloat("New Float", -1.0f);
        //}
    }

    private void GetFish()
    {
        _text.SetActive(true);
        _text.GetComponent<Text>().text = "Нажимайте E";
        StartCoroutine("GetFishGame");
        Invoke("StopGetFishGame", Random.Range(10, 21) / 10.0f);
    }

    IEnumerator GetFishGame()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                ++_count;
            }
            yield return null; //new WaitForFixedUpdate(); //Seconds(0.01f);
        }
    }

    private void StopGetFishGame()
    {
        StopCoroutine("GetFishGame");

        float formula = _count * Random.Range(70, 85) / 100.0f;
        if (formula >= 2) // 5 под вопросом, коэффициент нужно изменить
        {
            _text.GetComponent<Text>().text = "Вы поймали рыбу!"; // Вы поймали рыбу!
            Item item = new Item(fish);
            inventory.AddItem(item, 1);

            _animator.SetTrigger("FishingEnd");
        }
        else
        {
            _text.GetComponent<Text>().text = "Вы упустили рыбу!"; // Вы упустили рыбу!
            _animator.SetTrigger("Idle");
        }
        Invoke("HideText", 5.0f);

        // Закончили рыбалку
        CharacterMoving.IsReadyToMove = true;
        _player.GetComponent<Battle>().AllowBattle = true;
        _readyToFishing = false;
        NowFishing = false;

        //print("Count: " + _count + ". Формула: " + formula);
    }

    private void HideText()
    {
        _text.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            _readyToFishing = true;
        }
    }
}
