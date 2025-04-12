using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPBoxOnReset : MonoBehaviour
{
    [SerializeField] private Transform targetObject; //Objeto a mover
    [SerializeField] private Transform destinationPoint; // Objeto vacio que representa la ubicacion
    [SerializeField] private KeyCode keyToPress = KeyCode.R;
    private void Update()
    {
        if (Input.GetKeyDown(keyToPress))
        {
            if (targetObject != null && destinationPoint != null)
            {

                targetObject.position = destinationPoint.position;

            }
        }
    }

}
