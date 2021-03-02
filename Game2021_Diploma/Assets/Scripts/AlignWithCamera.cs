using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignWithCamera : MonoBehaviour
{
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private float _size;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = _cameraTransform.rotation;
        transform.position = _cameraTransform.position + transform.forward * _size; //RayToTarget._ray.direction;
        
    }
}
