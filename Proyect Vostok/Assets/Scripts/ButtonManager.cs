using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ButtonManager : MonoBehaviour
{

    public void GoToMenu()
    {
        LevelManager.instance.LoadLevel(0);
        DeathCounter.instance.contadorMuertes = 0;
    }
    public void ButtonPlay()
    {
        LevelManager.instance.LoadLevel(1);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    //Levels
    public void Level1()
    {
        LevelManager.instance.LoadLevel(1);
    }

    public void Level2()
    {
        LevelManager.instance.LoadLevel(2);
    }

    public void Level3()
    {
        LevelManager.instance.LoadLevel(3);
    }

    public void LevelSelect()
    {
        LevelManager.instance.LoadLevel(6);
    }

}
