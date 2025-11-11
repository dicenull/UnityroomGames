using R3;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Canvas canvas;

    void Start()
    {
        Time.timeScale = 1f;
        GameData.Instance.IsGameOver.Subscribe(isGameOver =>
        {
            if (isGameOver)
            {
                GameOver();
            }
            else
            {
                GameStart();
            }
        }).AddTo(this);
    }

    void GameStart()
    {
        canvas.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    void GameOver()
    {
        canvas.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }
}
