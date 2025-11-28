using R3;
using TMPro;
using UnityEngine;

public class DiceControlTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;

    void Start()
    {
        var gameData = GetIt.Instance.Get<DiceControlGameData>();
        gameData.TimeScore.Subscribe(timeScore =>
        {
            // 00:00の形式
            var minutes = (int)(timeScore / 60);
            var seconds = (int)(timeScore % 60);

            timerText.text = $"{minutes:00}:{seconds:00}";
        }).AddTo(this);
    }

    void Update()
    {
        var gameData = GetIt.Instance.Get<DiceControlGameData>();
        if (gameData.IsGameStart.Value)
        {
            gameData.TimeScore.Value += Time.deltaTime;
        }
    }
}
