
using System;
using R3;
using UnityEngine;

public class ChargeEnemyPlayer : MonoBehaviour
{
    void Start()
    {
        var data = GetIt.Instance.Get<ChargeGameData>();
        data.CurrentHand.Subscribe(hand =>
        {
            if (hand == null)
            {
                return;
            }
            var hands = Enum.GetValues(typeof(ChargeHands));
            var length = hands.Length;
            var randomIndex = UnityEngine.Random.Range(0, length);
            var randomHand = (ChargeHands)hands.GetValue(randomIndex);

            data.SetEnemyHand(randomHand);
        }).AddTo(this);
    }

}
