
using R3;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public ReactiveProperty<bool> IsGameOver = new(false);
    public static GameData Instance { get; private set; }

    GameData()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void GameOver()
    {
        IsGameOver.Value = true;
    }

    public void Reset()
    {
        IsGameOver.Value = false;
    }
}
