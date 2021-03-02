using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Weapon
{
    None = 0,
    Khife = 1,
    Sword = 2,
    Bow = 3
}

public class WeaponEnum : MonoBehaviour
{
    static public Weapon _selectedWeapon;
}