using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeClothes : MonoBehaviour
{
    // 0
    [SerializeField] private GameObject _leatherBelt;
    [SerializeField] private GameObject _shoes;
    // 1
    [SerializeField] private GameObject _top1;
    [SerializeField] private GameObject _bot1;
    // 2
    [SerializeField] private GameObject _top2;
    [SerializeField] private GameObject _top2_0;
    [SerializeField] private GameObject _bot2;
    [SerializeField] private GameObject _button1;
    [SerializeField] private GameObject _button2;
    [SerializeField] private GameObject _button3;
    [SerializeField] private GameObject _button4;
    // 3
    [SerializeField] private GameObject _top3;
    [SerializeField] private GameObject _bot3;

    private static GameObject[] _allClothers;
    private static GameObject[] _firstClothers;
    private static GameObject[] _secondClothers;
    private static GameObject[] _thirdClothers;

    private void Awake()
    {
        _allClothers = new GameObject[] { _top1, _bot1, _top2, _top2_0, _bot2, _button1, _button2, _button3, _button4, _top3, _bot3 };
        _firstClothers = new GameObject[] { _top1, _bot1 };
        _secondClothers = new GameObject[] { _top2, _top2_0, _bot2, _button1, _button2, _button3, _button4 };
        _thirdClothers = new GameObject[] { _top3, _bot3 };
    }

    public static void Change(int i)
    {
        Clothe(_allClothers, false);

        switch (i)
        {
            case 1:
                Clothe(_firstClothers, true);
                break;
            case 2:
                Clothe(_secondClothers, true);
                break;
            case 3:
                Clothe(_thirdClothers, true);
                break;
            default:
                break;
        }
    }

    private static void Clothe(GameObject[] clothers, bool active)
    {
        foreach (GameObject item in clothers)
        {
            item.SetActive(active);
        }
    }
}
