
using UnityEngine;
using UnityEngine.EventSystems;

public class RestartButton : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        RestartGame();
    }

    private void RestartGame()
    {
        TempGameData.Instance.Reset();
        Time.timeScale = 1f; // タイムスケールを元に戻す
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }
}