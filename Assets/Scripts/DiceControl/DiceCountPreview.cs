using UnityEngine;
using R3;
using TMPro;
using System.Collections.Generic;

public class DiceCountPreview : MonoBehaviour
{

    void Start()
    {
        var gameData = GetIt.Instance.Get<DiceControlGameData>();

        var diceCount = new Dictionary<ReactiveProperty<int>, Transform>
        {
            {gameData.DiceCountOne, transform.GetChild(0)},
            {gameData.DiceCountTwo, transform.GetChild(1)},
            {gameData.DiceCountThree, transform.GetChild(2)},
            {gameData.DiceCountFour, transform.GetChild(3)},
            {gameData.DiceCountFive, transform.GetChild(4)},
            {gameData.DiceCountSix, transform.GetChild(5)},
        };

        foreach (var kvp in diceCount)
        {
            kvp.Key.Subscribe(count =>
                     {
                         var text = kvp.Value.GetComponentInChildren<TextMeshProUGUI>();
                         text.text = $"{count}";
                     }).AddTo(this);
        }

    }

}
