
using R3;

public interface IGameData
{
    public Observable<bool> IsGameOver { get; }
    public void GameOver();
    public void Reset();
}