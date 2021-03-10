using System.Reflection;
using System.Diagnostics;
using System.Security.AccessControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShowHideInventory : MonoBehaviour
{
    [SerializeField]
    private GameObject _character;
    [SerializeField]
    private GameObject _characterCamera;
    [SerializeField]
    private GameObject _characterInventory;
    public event EventHandler OnIPressed;
    public bool _showHideInventory = false;

    private void Start()
    {
        OnIPressed += Test;
    }

    private void Test(object sender, EventArgs e)
    {
        UnityEngine.Debug.Log("I Pressed!");
        _showHideInventory = !_showHideInventory;   

        if(_showHideInventory == true)
        {
            Cursor.lockState = CursorLockMode.None;
            _characterInventory.GetComponent<Canvas>().enabled = true;
            _characterCamera.SetActive(false);            
            _character.GetComponent<CharacterMoving>().enabled = false;
            //_character.GetComponent<CharacterController>().enabled = false;
            //_character.GetComponent<Battle>().enabled = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            _characterInventory.GetComponent<Canvas>().enabled = false;
            _characterCamera.SetActive(true);           
            _character.GetComponent<CharacterMoving>().enabled = true;
            //_character.GetComponent<CharacterController>().enabled = true;
            //_character.GetComponent<Battle>().enabled = true;
        }
    }

    private void Update()
    {
       if (Input.GetKeyDown(KeyCode.I))
        {
            OnIPressed?.Invoke(this, EventArgs.Empty);
                                   
        } 
    }
    
}
