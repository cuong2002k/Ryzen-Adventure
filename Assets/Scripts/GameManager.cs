using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{

    public Vector3 Posplayer;
    public static Action<GameState> gameStateEvent;
    public static GameManager instance;

    public GameState state;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else Destroy(this.gameObject);

    }

    private void Start()
    {
        UpdateOnStateGame(GameState.Play);
    }

    public void UpdateOnStateGame(GameState newstate)
    {
        state = newstate;
        switch (state)
        {
            case GameState.Play:
                break;
            case GameState.RestartGame:
                RestartGame();
                break;

        }
        gameStateEvent?.Invoke(state);
    }

    public void SetPosition(Vector3 position)
    {
        Posplayer = position;
    }

    private void RestartGame()
    {
        StartCoroutine(DelayRestartGame());
    }

    IEnumerator DelayRestartGame()
    {
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

public enum GameState
{
    Play,
    RestartGame,
    SetPostion


}
