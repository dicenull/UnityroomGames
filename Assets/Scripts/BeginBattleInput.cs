using UnityEngine;
using UnityEngine.InputSystem;

public class BeginBattleInput : MonoBehaviour
{
    private NameInputManager nameInputManager;

    private void Awake()
    {
        nameInputManager = FindObjectOfType<NameInputManager>();
    }

    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard[Key.Enter].wasPressedThisFrame)
        {
            nameInputManager.OnBeginBattleClicked();
        }
    }
}
