using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextAppear : MonoBehaviour
{
    private float currentTime;
    public float timer = 30;
    public GameObject _texto;

    void Update()
    {
        currentTime += Time.deltaTime;
        FinishTime();
    }

    private void FinishTime()
    {
        if (currentTime == timer)
        {
            _texto.SetActive(false);
        }
    }
}
