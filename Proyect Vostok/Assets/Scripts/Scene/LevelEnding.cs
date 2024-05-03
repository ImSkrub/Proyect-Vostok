using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.Examples.TMP_ExampleScript_01;

public class LevelEnding : MonoBehaviour
{
    GameObject childPopUp;
    IEnumerator ShowObjectForTime(float time)
    {
        childPopUp.SetActive(true);
        yield return new WaitForSeconds(time);
        childPopUp.SetActive(false);
        Destroy(childPopUp);
        SceneLoader.Load(SceneLoader.Scene.MainMenu);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Transform child = this.gameObject.transform.GetChild(0);
        childPopUp = child.gameObject;
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(ShowObjectForTime(3f));

        }

    }
}
