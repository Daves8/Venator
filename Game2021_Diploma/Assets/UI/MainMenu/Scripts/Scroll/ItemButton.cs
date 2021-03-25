using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class ItemButton: MonoBehaviour
{
	public Button mainButton; // основная кнопка, на который будет отображено название объекта
	public TextMeshProUGUI mainButtonText; // текст главной кнопки
	public Button removeButton; // дочерняя кнопка, которая удаляет главную
    public string savePath;

    public ItemButton(){}
	public void PrintBtn()
	{
		Debug.Log("Вот Эта кнопка была нажата "+ savePath);
	}
}