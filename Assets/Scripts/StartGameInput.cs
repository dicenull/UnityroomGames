using UnityEngine;
using UnityEngine.InputSystem;

public class StartGameInput : MonoBehaviour
{
    private NameInputManager nameInputManager;

    private void Awake()
    {
        nameInputManager = FindObjectOfType<NameInputManager>();
    }

    public void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard[Key.Enter].wasPressedThisFrame)
        {
            nameInputManager.OnStartGameClicked();
        }
    }
}
