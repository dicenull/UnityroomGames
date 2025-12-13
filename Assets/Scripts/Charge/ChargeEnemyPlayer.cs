
using System;
using System.Linq;
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

            // チャージが足りない場合はビームを出せないようにする
            var hands = Enum.GetValues(typeof(ChargeHands))
                .Cast<ChargeHands>().Where(h => data.EnemyCharge.Value > 0 || h != ChargeHands.Beam).ToList();
            var length = hands.Count;
            var randomIndex = UnityEngine.Random.Range(0, length);
            var randomHand = hands[randomIndex];

            data.SetEnemyHand(randomHand);
        }).AddTo(this);
    }

}
