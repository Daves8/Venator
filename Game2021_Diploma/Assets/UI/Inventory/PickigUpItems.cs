using System.Web;
using System.Net.Http;
using System.Net.WebSockets;
using System.Net;
using System.Reflection;
using System.Net.NetworkInformation;
using System.Drawing;
using System.Transactions;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;



public class PickigUpItems : MonoBehaviour
{

    private int _playerLayerMask = 2;
    private float _rayCastMaxDistance = 6F;
    private bool showHelper = false;

    public InventoryObject inventory;

    public Transform Pointer;

    public Canvas showHelpCanvas;

    public GameObject showHelpObj;
    public GameObject showPickedItemObj;
    public TextMeshProUGUI showHelp;
    public TextMeshProUGUI showPickedItem;

    private Fishing _river;
    private PlayerCharacteristics _playerCharact;
    private GameObject _cart;
    private GameObject _forestCart;


    private void Start()
    {
        _playerCharact = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacteristics>();
        _cart = GameObject.FindGameObjectWithTag("Cart");
        _forestCart = GameObject.FindGameObjectWithTag("ForestCart");
    }

    IEnumerator ShowPickedItemCourutine()
    {
        showPickedItemObj.SetActive(true);
        showPickedItemObj.SetActive(true);
        yield return new WaitForSeconds(0.55f);
        showHelper = false;
        showPickedItemObj.SetActive(false);

        _river = GameObject.FindGameObjectWithTag("River").GetComponent<Fishing>();
    }

    void LateUpdate()
    {
        if (showHelper) StartCoroutine(ShowPickedItemCourutine());

        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);

        Physics.Raycast(ray, out hit, _rayCastMaxDistance, _playerLayerMask);


        Debug.DrawRay(transform.position, transform.forward * _rayCastMaxDistance, UnityEngine.Color.blue);

        float distanceTohit = Vector3.Distance(Pointer.position, transform.position);



        if (Physics.Raycast(ray, out hit))
        {
            var groundItem = hit.collider.gameObject.GetComponent<GroundItem>();
            Pointer.position = hit.point;
            //Debug.Log($"Distance to hit: {distanceTohit},Z{hit.point.z}X{hit.point.x}Y{hit.point.y}");
            if (groundItem)
            {
                if (groundItem.item.ruName == "Перемещение")
                {
                    if(Vector3.Distance(_playerCharact.gameObject.transform.position, _cart.transform.position) < 2f || Vector3.Distance(_playerCharact.gameObject.transform.position, _forestCart.transform.position) < 2f)
                    {
                        showHelpObj.SetActive(true);
                        showHelp.text = "Нажмите F – " + groundItem.item.ruName;
                        Invoke("HidePress", 3f);
                        if (Input.GetKeyDown(KeyCode.F))
                        {
                            _playerCharact.Teleport();
                        }
                    }
                    return;
                }
                //Debug.Log("навел на предмет");
                if (distanceTohit <= _rayCastMaxDistance)
                {
                    showHelpObj.SetActive(true);
                    showHelp.text = "Нажмите F – " + groundItem.item.ruName;
                    Invoke("HidePress", 3f);
                }
                if (groundItem && Input.GetKeyDown(KeyCode.F) && distanceTohit <= _rayCastMaxDistance)
                {

                    showHelper = true;
                    showPickedItem.text = "Вы подобрали " + groundItem.item.ruName;
                    Item _item = new Item(groundItem.item);
                    if (inventory.AddItem(_item, 1))
                    {
                        Destroy(hit.collider.gameObject);
                    }

                }
            }
            else
            {
                if (_river && !_river.NowFishing)
                {
                    showHelpObj.SetActive(false);
                }
            }
        }
    }
    private void HidePress()
    {
        showHelpObj.SetActive(false);
    }
}