using R3;

public class KawaGameData : IGameData
{
    public ReactiveProperty<bool> IsGameOver = new(false);
    public ReactiveProperty<bool> IsGameStart = new(false);

    public ReactiveProperty<float> TimeScore = new(0f);

    Observable<bool> IGameData.IsGameOver => IsGameOver;

    public void GameStart()
    {
        IsGameStart.Value = true;
    }

    public void GameOver()
    {
        IsGameOver.Value = true;
    }

    public void Reset()
    {
        IsGameOver.Value = false;
        IsGameStart.Value = false;
        TimeScore.Value = 0;
    }
}
