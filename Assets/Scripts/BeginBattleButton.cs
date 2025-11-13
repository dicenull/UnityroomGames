using UnityEngine;
using UnityEngine.EventSystems;

public class BeginBattleButton : MonoBehaviour, IPointerClickHandler
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
            nameInputManager.OnBeginBattleClicked();
        }
    }
}
