using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetPoint : MonoBehaviour
{
	public Transform target;
	public RectTransform pointerUI;
	public Sprite pointerIcon;
	public Sprite outOfScreenIcon;

	private float interfaceScale = 100;
	private Vector3 startPointerSize;
	private Camera mainCamera;
	private Transform player;

	private void Start()
	{
		startPointerSize = pointerUI.sizeDelta;
		mainCamera = Camera.main;

		player = GameObject.FindGameObjectWithTag("Player").transform;
	}
	private void LateUpdate()
	{
        if (target == null)
        {
			pointerUI.gameObject.SetActive(false);
			return;
        }
		pointerUI.gameObject.SetActive(true);

		Vector3 realPos = mainCamera.WorldToScreenPoint(target.position + new Vector3(0f, 2f, 0f)); // получениее экранных координат объекта
		Rect rect = new Rect(0, 0, Screen.width, Screen.height);

		Vector3 outPos = realPos;
		float direction = 1;

		pointerUI.GetComponent<Image>().sprite = outOfScreenIcon;

		if (!IsBehind(target.position + new Vector3(0f, 2f, 0f))) // если цель спереди
		{
			if (rect.Contains(realPos)) // и если цель в окне экрана
			{
				pointerUI.GetComponent<Image>().sprite = pointerIcon;
			}
		}
		else // если цель cзади
		{
			realPos = -realPos;
			outPos = new Vector3(realPos.x, 0, 0); // позиция иконки - снизу
			if (mainCamera.transform.position.y < target.position.y + 2f)
			{
				direction = -1;
				outPos.y = Screen.height; // позиция иконки - сверху				
			}
		}
		// ограничиваем позицию областью экрана
		float offset = pointerUI.sizeDelta.x / 2;
		outPos.x = Mathf.Clamp(outPos.x, offset, Screen.width - offset);
		outPos.y = Mathf.Clamp(outPos.y, offset, Screen.height - offset);

		Vector3 pos = realPos - outPos; // направление к цели из PointerUI 

		RotatePointer(direction * pos);

		pointerUI.sizeDelta = new Vector2(startPointerSize.x / 100 * interfaceScale, startPointerSize.y / 100 * interfaceScale);
		pointerUI.anchoredPosition = outPos;
	}
	private bool IsBehind(Vector3 point) // true если point сзади камеры
	{
		Vector3 forward = mainCamera.transform.TransformDirection(Vector3.forward);
		Vector3 toOther = point - mainCamera.transform.position;
		if (Vector3.Dot(forward, toOther) < 0) return true;
		return false;
	}
	private void RotatePointer(Vector2 direction) // поворачивает PointerUI в направление direction
	{
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		pointerUI.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}

	public void PointToTarget(Transform localTarget)
	{
		if (Vector3.Distance(player.position, localTarget.position) > 5f)
		{
			target = localTarget.transform;

			//target.position = localTarget.transform.position;
			//target.rotation = localTarget.transform.rotation;
			//target.localScale = localTarget.transform.localScale;
			//target.position += new Vector3(0f, 2f, 0f);
        }
        else
        {
			target = null;
        }
	}
}
