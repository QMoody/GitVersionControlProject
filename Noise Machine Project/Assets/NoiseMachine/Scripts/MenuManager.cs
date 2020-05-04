using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    
    public void PlayGame()
    {
        SceneManager.LoadScene("SceneWPlayerModel");
    }

    public void PlayGame2()
    {
        SceneManager.LoadScene("EndlessScene");
    }

    public void SettingsMenu()
    {
        SceneManager.LoadScene("SettingsMenu");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void AchievementMenu()
    {
        SceneManager.LoadScene("AchievementMenu");
    }

    private void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

}
