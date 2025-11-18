using UnityEngine;
using UnityEngine.EventSystems;

public class AttackButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private BattleManager battleManager;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (battleManager != null)
        {
            battleManager.PlayerAttack();
        }
        else
        {
            Debug.LogError("BattleManager is not assigned in AttackButton!");
        }
    }
}
