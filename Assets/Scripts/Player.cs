using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    void Update()
    {
        var key = Keyboard.current;
        if (key[Key.Space].wasPressedThisFrame)
        {
            GameData.Instance.IsGameOver.Value = true;
        }
    }
}
