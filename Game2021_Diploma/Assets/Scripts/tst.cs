using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tst : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            print("нажали - " + Time.time);
        }
        if (Input.GetButton("Fire1"))
        {
            print("держим - " + Time.time);
        }
        if (Input.GetButtonUp("Fire1"))
        {
            print("отпустили - " + Time.time);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject.name + " коллизия");
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.name + " триггер");
    }
}