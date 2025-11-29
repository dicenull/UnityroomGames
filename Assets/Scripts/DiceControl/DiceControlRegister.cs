using R3;
using UnityEngine;
using unityroom.Api;

public class DiceControlRegister : MonoBehaviour
{
    void Awake()
    {
        GetIt.Instance.Register<IGameData, DiceControlGameData>(new DiceControlGameData());
    }

    void Start()
    {
        var gameData = GetIt.Instance.Get<DiceControlGameData>();

        gameData.IsGameOver.Subscribe(isGameOver =>
        {
            if (isGameOver)
            {
                var score = gameData.TimeScore.Value;
                UnityroomApiClient.Instance.SendScore(1, score, ScoreboardWriteMode.HighScoreDesc);
            }
        }).AddTo(this);
    }
}
