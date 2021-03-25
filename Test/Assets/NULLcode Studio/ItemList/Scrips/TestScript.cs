// NULLcode Studio © 2016
// null-code.ru

// тестовый скрипт / для примера
using UnityEngine;
using System.Collections;

public class TestScript : MonoBehaviour {

	public ItemList itemList;
	public string[] itemTitle = new string[10];
	public int[] itemID = new int[10];

	void Start()
	{
	}

	public void AddNew()
	{
		int i = Random.Range(0, itemTitle.Length);
		itemList.AddToList(itemID[i], itemTitle[i], true);
	}

	public void Clear()
	{
		itemList.ClearItemList();
	}

}
