using UnityEngine;
using UnityEngine.InputSystem;

public class TempPlayer : MonoBehaviour
{

    void Update()
    {
        var gameData = GetIt.Instance.Get<IGameData>();
        if (Keyboard.current[Key.Space].wasPressedThisFrame)
        {
            gameData.GameOver();
        }
    }
}
