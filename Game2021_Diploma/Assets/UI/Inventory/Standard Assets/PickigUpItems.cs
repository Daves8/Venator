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
using UnityEngine;



public class PickigUpItems : MonoBehaviour
{
    
    private int _playerLayerMask = 2;
    private float _rayCastMaxDistance = 6F; 
    
    public InventoryObject inventory;
    
    public Transform Pointer;
    void LateUpdate()
    {
        
        RaycastHit hit;        
        Ray ray = new Ray(transform.position , transform.forward);

        Physics.Raycast(ray, out hit, _rayCastMaxDistance, _playerLayerMask);
        

        Debug.DrawRay(transform.position , transform.forward * _rayCastMaxDistance , Color.blue);
        
        float distanceTohit = Vector3.Distance(Pointer.position , transform.position);
        
        
        
        if(Physics.Raycast(ray, out hit))
        {
            var groundItem = hit.collider.gameObject.GetComponent<GroundItem>();
            Pointer.position = hit.point;
            Debug.Log($"Distance to hit: {distanceTohit},Z{hit.point.z}X{hit.point.x}Y{hit.point.y}");
            if(groundItem)
            Debug.Log("навел на предмет");
            if(groundItem && Input.GetKeyDown(KeyCode.E) && distanceTohit <= _rayCastMaxDistance)
            {                
                Item _item = new Item(groundItem.item);
                if (inventory.AddItem(_item, 1))
                {
                    Destroy(hit.collider.gameObject);
                } 
            }
        }
    }
}
