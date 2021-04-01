using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Delete", 300.0f);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 20, Color.blue);
    }

    private void OnTriggerEnter(Collider other)
    {
        //print("Стрела попала в " + other.gameObject.name);
        //if (other.gameObject.tag == "Enemy")
        //{
        //    other.gameObject.GetComponent<Enemy>()._hp -= 100;
        //    Invoke("Delete", 300.0f);
        //}
        //GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }

    private void Delete()
    {
        Destroy(gameObject);
    }
}
