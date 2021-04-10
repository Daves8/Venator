﻿using System.Threading;
using System.Reflection;
using System.Diagnostics;
using System.Security.AccessControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShowHideUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _character;
    [SerializeField]
    private GameObject _characterCamera;
    [SerializeField]
    private GameObject _characterInventory;//Canvas
    [SerializeField]
    
    private GameObject _PauseMenu;//Canvas
    [SerializeField]
    private GameObject _MainMenu;//Canvas

    public event EventHandler OnIPressed;
    public event EventHandler OnEscapePressed;
    public bool _showHideInventory = false;
    public bool _showHidePauseMenu = false;

    private void Start()
    {
        OnIPressed += ShowHideInventory;
        OnEscapePressed += ShowHidePauseMenu;        
    }

    private void Update()
    {
       if (Input.GetKeyDown(KeyCode.I))
        {
            OnIPressed?.Invoke(this, EventArgs.Empty);                                   
        }
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
               ShowHidePause();                                            
        } 
    }

    private void ShowHideInventory(object sender, EventArgs e)
    {
        UnityEngine.Debug.Log("I Pressed!");
        _showHideInventory = !_showHideInventory;   

        if(_showHideInventory == true)
        {
            Cursor.lockState = CursorLockMode.None;
            _characterInventory.GetComponent<Canvas>().enabled = true;
            _characterCamera.SetActive(false);            
            _character.GetComponent<CharacterMoving>().enabled = false;            
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            _characterInventory.GetComponent<Canvas>().enabled = false;
            _characterCamera.SetActive(true);           
            _character.GetComponent<CharacterMoving>().enabled = true;            
        }
    }

    private void ShowHidePauseMenu(object sender, EventArgs e)
    {
        //UnityEngine.Debug.Log("Escape Pressed!");--
        _showHidePauseMenu= !_showHidePauseMenu;

        if(_showHidePauseMenu == true)
        {
            Cursor.lockState = CursorLockMode.None;
            _PauseMenu.GetComponent<Canvas>().enabled = true;
            Time.timeScale = 0;         
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            _PauseMenu.GetComponent<Canvas>().enabled = false;
            Time.timeScale = 1;            
        }
    }
    
    public void ShowHidePause()
    {
        OnEscapePressed?.Invoke(this, EventArgs.Empty);
    }
    
}
