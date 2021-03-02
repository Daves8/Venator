using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayToTarget : MonoBehaviour
{
    static internal Ray _ray;

    // Start is called before the first frame update
    void Start()
    {
        _ray = new Ray(transform.position, transform.forward);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 30, Color.yellow);
    }
}
