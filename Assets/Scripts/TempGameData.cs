using R3;
using UnityEngine;

public class TempGameData : MonoBehaviour
{
    public ReactiveProperty<bool> IsGameOver = new(false);

    public static TempGameData Instance { get; private set; }

    TempGameData()
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

    public void Reset()
    {
        IsGameOver.Value = false;
    }
}
