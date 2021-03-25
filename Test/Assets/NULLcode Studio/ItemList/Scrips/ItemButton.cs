// NULLcode Studio © 2016
// null-code.ru

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemButton : MonoBehaviour {

	public int id; // идентификатор объекта, который храниться в списке
	public Button mainButton; // основная кнопка, на который будет отображено название объекта
	public Text mainButtonText; // текст главной кнопки
	public Button removeButton; // дочерняя кнопка, которая удаляет главную

}
