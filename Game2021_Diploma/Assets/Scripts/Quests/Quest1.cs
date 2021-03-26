using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quest1 : MonoBehaviour
{
    public bool QuestActive;
    public GameObject brother;
    public GameObject player;
    public Vector3 startPosition;
    public Text subtitles;
    public Text prompt;
    public Text target;
    public Cinemachine.CinemachineVirtualCamera groupCamera;
    public Cinemachine.CinemachineTargetGroup targetGroup;
    public Subquest subquest;

    public enum Subquest
    {
        none = 0,
        subquest1 = 1, // Начало. Первый диалог с братом.
        subquest2, // Диалог окончен. Задача: выбрать из инвентаря удочку и начать рыбачить.
        subquest3, // Начата рыбалка. Задача: поймать рыбу.
        subquest4, // 1 рыба поймана. Задача: подойти к брату.
        subquest5, // Подошли к брату. Выбор: уходим или ловим дальше (уходим - карма на 0, словили 2 рыбы - карма +, словили 2+ рыбы - карма++).
        subquest6 // Рыбалка окончена. Задача: тдти с братом в деревню.
    }

    // Start is called before the first frame update
    void Start()
    {
        brother.transform.position = startPosition;
        player.transform.position = startPosition + new Vector3(-1f, 0, -1f);

        subtitles.text = "";
        prompt.text = "";

        subquest = Subquest.none;

        groupCamera.enabled = false;
        targetGroup.m_Targets = new Cinemachine.CinemachineTargetGroup.Target[] { new Cinemachine.CinemachineTargetGroup.Target { target = player.transform, weight = 1f, radius = 0f }, new Cinemachine.CinemachineTargetGroup.Target { target = brother.transform, weight = 1f, radius = 0f } };
    }

    // Update is called once per frame
    void Update()
    {
        if (!QuestActive)
        {
            return;
        }

        switch (subquest)
        {
            case Subquest.subquest1:
                SubQ1();
                break;
            case Subquest.subquest2:
                SubQ2();
                break;
            case Subquest.subquest3:
                SubQ3();
                break;
            case Subquest.subquest4:
                SubQ4();
                break;
            case Subquest.subquest5:
                SubQ5();
                break;
            case Subquest.subquest6:
                SubQ6();
                break;
            default:
                groupCamera.enabled = false;
                break;
        }
        //enabled = false;
    }
    private void SubQ1() // Начало. Первый диалог с братом.
    {
        target.text = "Поговорить с братом.";
        groupCamera.enabled = true;
        subtitles.text = "Начало. Восход Солнца. Камера медленно передвигается по коючевым локациям игры. Камера идет в небо, " +
            "появляется название игры. Идеи: При самом начале игры последней локацией над которой будет пролетать камера будет " +
            "речка с ГГ и его младшим братом.При нажатии кнопки \"Новая Игра\" игра начинается без загрузки с этого же места: рыбалка.";

        //subquest = Subquest.subquest2;
    }
    private void SubQ2() // Диалог окончен. Задача: выбрать из инвентаря удочку и начать рыбачить.
    {
        target.text = "Начать рыбачить.";

        if (true/*проверка инвентаря на наличие рыбы*/)
        {
            subquest = Subquest.subquest3;
        }
    }
    private void SubQ3() // Начата рыбалка. Задача: поймать рыбу.
    {
        target.text = "Поймать рыбу.";

        if (true/*проверка инвентаря на наличие рыбы*/)
        {
            subquest = Subquest.subquest4;
        }
    }
    private void SubQ4() // 1 рыба поймана. Задача: подойти к брату.
    {
        target.text = "Подойти к брату.";
        //subquest = Subquest.subquest5;
    }
    private void SubQ5() // Подошли к брату. Выбор: уходим или ловим дальше (уходим - карма на 0, словили 2 рыбы - карма +, словили 2+ рыбы - карма++).
    {
        target.text = "Наловить еще рыбы.";
        //subquest = Subquest.subquest6;
    }
    private void SubQ6() // Рыбалка окончена. Задача: тдти с братом в деревню.
    {
        target.text = "Пойти в деревню.";
        //subquest = Subquest.none;
    }
}