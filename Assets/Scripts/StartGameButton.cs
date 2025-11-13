using UnityEngine;
using UnityEngine.EventSystems;

public class StartGameButton : MonoBehaviour, IPointerClickHandler
{
    private NameInputManager nameInputManager;

    private void Awake()
    {
        nameInputManager = FindObjectOfType<NameInputManager>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (nameInputManager != null)
        {
            nameInputManager.OnStartGameClicked();
        }
    }
}
