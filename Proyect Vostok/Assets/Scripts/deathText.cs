using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class deathText : MonoBehaviour
{
    public TextMeshProUGUI textoContadorMuertes;

    void Start()
    {
        ActualizarTextoContadorMuertes();
    }

    void ActualizarTextoContadorMuertes()
    {
      textoContadorMuertes.text = "Intentos: " +  DeathCounter.instance.contadorMuertes.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        ActualizarTextoContadorMuertes();
    }



}
