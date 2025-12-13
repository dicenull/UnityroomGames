using System;
using R3;

public enum ChargeGamePhase
{
    Waiting,
    Judge,
    GameOver
}

public class ChargeGameData : IGameData
{
    public ReactiveProperty<ChargeHands?> CurrentHand = new(null);
    public ReactiveProperty<ChargeHands?> EnemyHand = new(null);

    public ReactiveProperty<ChargeGamePhase> CurrentPhase = new(ChargeGamePhase.Waiting);
    public ReactiveProperty<int> Turn = new(0);
    public ReactiveProperty<int> PlayerCharge = new(0);
    public ReactiveProperty<int> EnemyCharge = new(0);
    public ReactiveProperty<bool?> IsWin = new(null);

    Observable<bool> IGameData.IsGameOver => CurrentPhase.Select(phase => phase == ChargeGamePhase.GameOver);

    public void GameOver()
    {
        CurrentPhase.Value = ChargeGamePhase.GameOver;
    }

    public void Reset()
    {
        Turn.Value = 0;
        PlayerCharge.Value = 0;
        EnemyCharge.Value = 0;
        CurrentPhase.Value = ChargeGamePhase.Waiting;
    }

    public void JudgeByHand(ChargeHands hand)
    {
        if (CurrentPhase.Value != ChargeGamePhase.Waiting)
        {
            throw new InvalidOperationException("判定は待機フェーズでのみ可能");
        }

        CurrentPhase.Value = ChargeGamePhase.Judge;
        CurrentHand.Value = hand;
    }

    public void SetEnemyHand(ChargeHands hand)
    {
        EnemyHand.Value = hand;
    }

    public void NextTurn(JudgeResult result)
    {
        Turn.Value += 1;

        if (CurrentHand.Value == ChargeHands.Charge)
        {
            PlayerCharge.Value += 1;
        }
        if (CurrentHand.Value == ChargeHands.Beam)
        {
            PlayerCharge.Value -= 1;
        }
        if (EnemyHand.Value == ChargeHands.Charge)
        {
            EnemyCharge.Value += 1;
        }
        if (EnemyHand.Value == ChargeHands.Beam)
        {
            EnemyCharge.Value -= 1;
        }

        CurrentHand.Value = null;
        EnemyHand.Value = null;

        IsWin.Value = result switch
        {
            JudgeResult.Win => true,
            JudgeResult.Lose => false,
            JudgeResult.Draw => null,
            _ => throw new NotImplementedException(),
        };

        if (IsWin.Value != null)
        {
            CurrentPhase.Value = ChargeGamePhase.GameOver;
        }
        else
        {
            CurrentPhase.Value = ChargeGamePhase.Waiting;
        }

    }
}
