using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Vector3 _rbVel;

    void Start()
    {
        Invoke("Delete", 5.0f); // было 300.0f
        _rigidbody = GetComponent<Rigidbody>();
        _rbVel = _rigidbody.velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //print("Стрела попала в " + collision.gameObject.name);
        if (collision.gameObject.tag != "Player" && collision.gameObject.tag != "Arrow")
        {   
            //GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            //transform.SetParent(collision.transform);
        }
        //print(name + ": " + _rigidbody.velocity + " - " + _rbVel);
        //if (Mathf.Abs(_rigidbody.velocity.magnitude - _rbVel.magnitude) > 1)
        //{
        //    //GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        //    //transform.SetParent(collision.transform);
        //}
    }

    private void Delete()
    {
        Destroy(gameObject);
    }
}