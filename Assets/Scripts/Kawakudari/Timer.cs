using R3;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;

    void Start()
    {
        var gameData = GetIt.Instance.Get<KawaGameData>();
        gameData.TimeScore.Subscribe(timeScore =>
        {
            // 00:00:00の形式
            var minutes = (int)(timeScore / 60);
            var seconds = (int)(timeScore % 60);
            var milliseconds = (int)(timeScore * 100 % 100);
            timerText.text = $"{minutes:00}:{seconds:00}:{milliseconds:00}";
        }).AddTo(this);
    }
    void Update()
    {

        var gameData = GetIt.Instance.Get<KawaGameData>();
        if (gameData.IsGameStart.Value)
        {
            gameData.TimeScore.Value += Time.deltaTime;
        }
    }
}
