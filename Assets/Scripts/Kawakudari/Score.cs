using R3;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    void Start()
    {
        var gameData = GetIt.Instance.Get<KawaGameData>();
        gameData.FreezeScore.Subscribe(score =>
        {
            scoreText.text = score.ToString("F2");
        }).AddTo(this);
    }
}
