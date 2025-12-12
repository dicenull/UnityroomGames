using UnityEngine;
using UnityEngine.InputSystem;

public class InvPlayer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Update()
    {
        var gameData = GetIt.Instance.Get<IGameData>();
        var keyboard = Keyboard.current;
        if (keyboard[Key.Space].wasPressedThisFrame)
        {
            gameData.GameOver();
        }
    }
}
