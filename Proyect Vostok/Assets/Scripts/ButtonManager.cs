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
        PowerUpManager.Instance.powerUp.Clear();

    }
    public void RestartLevel()
    {
        LevelManager.instance.RestartLevel();
       // PowerUpManager.Instance.powerUp.Clear();
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
    public void Level4()
    {
        LevelManager.instance.LoadLevel(4);
    }
    public void Level5()
    {
        LevelManager.instance.LoadLevel(5);
    }
    public void LevelSelect()
    {
        LevelManager.instance.LoadLevel(8);
    }

}
