using UnityEngine;
using UnityEngine.EventSystems;

public class PotionButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private BattleManager battleManager;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (battleManager != null)
        {
            // Phase 4で実装予定
            Debug.Log("Potion button clicked - feature coming in Phase 4");
        }
        else
        {
            Debug.LogError("BattleManager is not assigned in PotionButton!");
        }
    }
}
