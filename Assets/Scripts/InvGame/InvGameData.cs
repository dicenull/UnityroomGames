
using System;
using R3;
using Unity.VisualScripting;
using UnityEngine;

public enum InvPhase
{
    Right,
    Left,
}

public class InvGameData : IGameData
{
    public ReactiveProperty<bool> IsGameOver = new(false);
    Observable<bool> IGameData.IsGameOver => IsGameOver;

    public ReactiveProperty<InvPhase> MovePhase = new(InvPhase.Right);
    BehaviorSubject<InvPhase> _hit = new(InvPhase.Right);

    public ReactiveProperty<float> ShotCoolDown = new(2f);

    public Observable<InvPhase> OnHit => _hit.AsObservable();

    public Vector3 MoveAmount => MovePhase.Value switch
    {
        InvPhase.Right => new Vector3(0.1f, 0, 0),
        InvPhase.Left => new Vector3(-0.1f, 0, 0),
        _ => throw new Exception("Invalid Phase")
    };

    public InvGameData()
    {
        var prevCoolDown = PlayerPrefs.GetFloat("CoolDown", 2f);
        ShotCoolDown.Value = prevCoolDown;
    }

    public void GameOver()
    {
        IsGameOver.Value = true;
    }

    public void Reset()
    {
        ShotCoolDown.Value -= .3f;
        PlayerPrefs.SetFloat("CoolDown", ShotCoolDown.Value);

        IsGameOver.Value = false;
        Debug.Log(ShotCoolDown.Value);
    }

    public void Hit(InvPhase phase)
    {
        _hit.OnNext(phase);
    }
}