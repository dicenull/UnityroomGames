using R3;

public class DiceControlGameData : IGameData
{
    public ReactiveProperty<bool> IsGameOver = new(false);
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
