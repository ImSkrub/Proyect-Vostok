using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ButtonManager : MonoBehaviour
{
    // Menú principal
    public void GoToMenu()
    {
        GameManager.Instance.ResetList(); // Limpieza general
        LevelManager.instance.LoadMainMenu();
        AudioManager.instance.StopAll();
        AudioManager.instance.PlayMusic("menu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ButtonPlay()
    {
        GameManager.Instance.ResetList();
        LevelManager.instance.LoadLevel(1); // Primer nivel jugable
        AudioManager.instance.StopAll();
        AudioManager.instance.PlayMusic("level");
    }

    public void LevelSelect()
    {
        LevelManager.instance.LoadLevel(8); // Suponiendo que la escena 8 es el selector
    }

    public void RestartLevel()
    {
        GameManager.Instance.ResetList(); // Reinicia copias y powerups
        LevelManager.instance.RestartLevel();
        AudioManager.instance.StopAll();
        AudioManager.instance.PlayMusic("level");
    }

    // Carga directa de niveles
    public void Level1() { CargarNivel(1); AudioManager.instance.StopAll(); AudioManager.instance.PlayMusic("level");}
    public void Level2() { CargarNivel(2); AudioManager.instance.StopAll(); AudioManager.instance.PlayMusic("level"); }
    public void Level3() { CargarNivel(3); AudioManager.instance.StopAll(); AudioManager.instance.PlayMusic("level"); }
    public void Level4() { CargarNivel(4); AudioManager.instance.StopAll(); AudioManager.instance.PlayMusic("level"); }
    public void Level5() { CargarNivel(5); AudioManager.instance.StopAll(); AudioManager.instance.PlayMusic("level"); }

    private void CargarNivel(int index)
    {
        GameManager.Instance.ResetList();
        LevelManager.instance.LoadLevel(index);
    }

}