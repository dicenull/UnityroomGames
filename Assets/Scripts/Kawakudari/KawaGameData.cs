using R3;

public class KawaGameData : IGameData
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
