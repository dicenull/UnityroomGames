using R3;


public class ChargeGameData : IGameData
{
    public ReactiveProperty<bool> IsGameOver { get; } = new ReactiveProperty<bool>(false);
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
