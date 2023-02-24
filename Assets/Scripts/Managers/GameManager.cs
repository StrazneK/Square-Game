using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState gameState;
    public bool myTurn = true;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        gameState = GameState.Pause;
    }
    public void StartGame()
    {
        gameState = GameState.Play;
        TapControl.instance.StartGame();
    }
    public enum GameState
    {
        Play,
        Pause
    }
}
