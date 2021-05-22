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

    private void OnCollisionEnter(Collision collision)
    {
        print("Стрела попала в " + collision.gameObject.name);
        if (collision.gameObject.tag != "Player" && collision.gameObject.tag != "Arrow")
        {   
            //GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            //transform.SetParent(collision.transform);
        }
    }

    private void Delete()
    {
        Destroy(gameObject);
    }
}