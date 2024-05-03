using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float multiplierEffect;
    Transform cameraTransform;
    Vector3 lastCameraPosition;

    void Start()
    {
        //Guarda referencia del transform a la camara principal
        cameraTransform = Camera.main.transform;
        //Guardo la ultima posicion
        lastCameraPosition = cameraTransform.position;
    }

    void Update()
    {
        Vector3 newPos = cameraTransform.position - lastCameraPosition;

        transform.position += new Vector3 (newPos.x * multiplierEffect,0f,0f) ; 

        lastCameraPosition = cameraTransform.position;
    }
}
