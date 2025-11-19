using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    void Update()
    {
        var key = Keyboard.current;
        if (key[Key.Space].wasPressedThisFrame)
        {
            TempGameData.Instance.IsGameOver.Value = true;
        }
    }
}
