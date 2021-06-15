using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Fishing : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _river;
    [SerializeField] private GameObject _player;
    //[SerializeField] private GameObject _text;

    public InventoryObject inventory;
    public ItemObject fish;
    public GameObject rod;
    public TextMeshProUGUI showPickeditem;
    public TextMeshProUGUI showEnterF;

    private bool _readyToFishing = false;
    private bool _startFishing = false;
    private bool _fishingOutCome = false;
    public bool NowFishing = false;

    private bool _nextDo = false;

    private int _count = 0;

    private void Start()
    {
        showEnterF.text = "";
        showPickeditem.text = "";
        rod.SetActive(false);
    }

    private void Update()
    {
        if (_readyToFishing && !NowFishing && Input.GetButtonDown("Action"))
        {
            rod.SetActive(true);
            _player.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
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
        showEnterF.gameObject.SetActive(true);
        showEnterF.text = "Нажимайте F";
        StartCoroutine("GetFishGame");
        Invoke("StopGetFishGame", Random.Range(10, 21) / 10.0f);
    }

    IEnumerator GetFishGame()
    {
        while (true)
        {
            if (Input.GetButtonDown("Action"))
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
        if (formula >= 4.5f) // 5 под вопросом, коэффициент нужно изменить
        {
            //_text.GetComponent<TextMeshProUGUI>().text = "Вы поймали рыбу!"; // Вы поймали рыбу!
            showEnterF.gameObject.SetActive(false);
            showPickeditem.gameObject.SetActive(true);
            showPickeditem.text = "Вы поймали рыбу!";
            Item item = new Item(fish);
            inventory.AddItem(item, 1);
            _fishingOutCome = true;
            _animator.SetTrigger("FishingEnd");
        }
        else
        {
            //_text.GetComponent<TextMeshProUGUI>().text = "Вы упустили рыбу!"; // Вы упустили рыбу!
            showEnterF.gameObject.SetActive(false);
            showPickeditem.gameObject.SetActive(true);
            showPickeditem.text = "Вы упустили рыбу!";
            _fishingOutCome = false;
            rod.SetActive(false);
            _animator.SetTrigger("Idle");
        }
        Invoke("HideText", 5.0f);

        // Закончили рыбалку
        CharacterMoving.IsReadyToMove = true;
        _player.GetComponent<Battle>().AllowBattle = true;
        _readyToFishing = false;
        Invoke("NowFishingChg", 1.0f);

        //print("Count: " + _count + ". Формула: " + formula);
    }
    private void NowFishingChg()
    {
        NowFishing = false;
        if (_fishingOutCome)
        {
            _fishingOutCome = false;
            rod.SetActive(false);
        }
    }
    private void HideText()
    {
        showPickeditem.gameObject.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            _readyToFishing = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        _readyToFishing = false;
    }
}