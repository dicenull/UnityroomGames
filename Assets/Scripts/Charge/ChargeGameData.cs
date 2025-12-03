using R3;


public class ChargeGameData : IGameData
{
    public ReactiveProperty<bool> IsGameOver = new(false);
    public ReactiveProperty<ChargeHands> CurrentHand = new(ChargeHands.Charge);
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
