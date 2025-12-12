using System;
using UnityEngine;

public class InvDiceController : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        var gameData = GetIt.Instance.Get<InvGameData>();
        var nextPhase = other.name switch
        {
            "RightBar" => InvPhase.Left,
            "LeftBar" => InvPhase.Right,
            _ => throw new Exception("Invalid Name")
        };
        gameData.Hit(nextPhase);
    }
}
