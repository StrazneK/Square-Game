using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] Transform winPanel;
    [SerializeField] Transform losePanel;
    [SerializeField] Transform startPanel;
    LevelController levelController;
    public static MenuController instance;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        levelController = LevelController.instance;
    }
    public void LosePanel()
    {
        losePanel.gameObject.SetActive(true);
        StartCoroutine(RestartWithSec());
    }
    IEnumerator RestartWithSec()
    {
        yield return new WaitForSeconds(2.5f);
        levelController.RestartLevel();
    }
    public void WinPanel()
    {
        winPanel.gameObject.SetActive(true);
    }
    public void GameStartButton()
    {
        startPanel.gameObject.SetActive(false);
        GameManager.instance.StartGame();
    }

}
