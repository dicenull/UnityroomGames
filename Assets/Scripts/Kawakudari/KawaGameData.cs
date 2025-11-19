using R3;
using UnityEngine;

public class KawaGameData : MonoBehaviour, IGameData
{
    public ReactiveProperty<bool> IsGameOver = new();

    Observable<bool> IGameData.IsGameOver => IsGameOver;

    public void GameOver()
    {
        IsGameOver.Value = true;
    }

    public void Reset()
    {
        IsGameOver.Value = false;
    }
}
