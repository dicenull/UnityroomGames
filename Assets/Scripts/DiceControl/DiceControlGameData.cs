using R3;

public class DiceControlGameData : IGameData
{
    public ReactiveProperty<bool> IsGameOver = new(false);
    public ReactiveProperty<int> DiceCountOne = new(5);
    public ReactiveProperty<int> DiceCountTwo = new(5);
    public ReactiveProperty<int> DiceCountThree = new(5);
    public ReactiveProperty<int> DiceCountFour = new(5);
    public ReactiveProperty<int> DiceCountFive = new(5);
    public ReactiveProperty<int> DiceCountSix = new(5);

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
