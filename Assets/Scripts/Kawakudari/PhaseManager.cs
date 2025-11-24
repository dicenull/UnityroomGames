using R3;
using UnityEngine;
using UnityEngine.InputSystem;
using unityroom.Api;

public class PhaseManager : MonoBehaviour
{

    [SerializeField] private GameObject startPhaseObject;

    void Start()
    {
        startPhaseObject.SetActive(true);

        GetIt.Instance.Get<IGameData>().IsGameOver.Subscribe(isGameOver =>
        {
            if (isGameOver)
            {
                var score = GetIt.Instance.Get<KawaGameData>().Score;
                UnityroomApiClient.Instance.SendScore(1, score, ScoreboardWriteMode.HighScoreDesc);
            }
        }).AddTo(this);
    }

    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard.enterKey.wasPressedThisFrame)
        {
            var gameData = GetIt.Instance.Get<KawaGameData>();
            gameData.GameStart();
            startPhaseObject.SetActive(false);
        }
    }
}
