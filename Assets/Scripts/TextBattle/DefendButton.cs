using UnityEngine;
using UnityEngine.EventSystems;

public class DefendButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private BattleManager battleManager;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (battleManager != null)
        {
            battleManager.PlayerDefend();
        }
        else
        {
            Debug.LogError("BattleManager is not assigned in DefendButton!");
        }
    }
}
