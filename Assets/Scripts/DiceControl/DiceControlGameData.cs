using R3;

public class DiceControlGameData : IGameData
{
    public ReactiveProperty<bool> IsGameOver = new(false);
    public ReactiveProperty<bool> IsGameStart = new(false);
    public ReactiveProperty<float> TimeScore = new(0f);
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

    public void AddDiceCount(int index)
    {
        switch (index)
        {
            case 1:
                DiceCountOne.Value += 1;
                break;
            case 2:
                DiceCountTwo.Value += 1;
                break;
            case 3:
                DiceCountThree.Value += 1;
                break;
            case 4:
                DiceCountFour.Value += 1;
                break;
            case 5:
                DiceCountFive.Value += 1;
                break;
            case 6:
                DiceCountSix.Value += 1;
                break;
        }
    }
}
