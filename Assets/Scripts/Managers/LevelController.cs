using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    int level = 1;
    public static LevelController instance;
    private void Awake()
    {
        instance = this;        
    }
    void Start()
    {
        TinySauce.OnGameStarted();
        level = PlayerPrefs.GetInt("Level", 1);
        if (!ControlLevel())
            OpenLevel();
    }

    public void NextLevel()
    {
        TinySauce.OnGameFinished(true, 0, level.ToString());
        level++;
        PlayerPrefs.SetInt("Level", level);
        OpenLevel();
    }

    public void RestartLevel()
    {
        TinySauce.OnGameFinished(false, 0, level.ToString());
        OpenLevel();
    }

    void OpenLevel()
    {
        int openingLevel = (level % SceneManager.sceneCountInBuildSettings);
        SceneManager.LoadScene(openingLevel == 0 ? SceneManager.sceneCountInBuildSettings - 1 : openingLevel - 1);
    }

    bool ControlLevel()
    {
        int openingLevel = (level % SceneManager.sceneCountInBuildSettings);
        if (openingLevel == 0)
            return SceneManager.sceneCountInBuildSettings - 1 == SceneManager.GetActiveScene().buildIndex;
        return openingLevel - 1 == SceneManager.GetActiveScene().buildIndex;
    }

}
