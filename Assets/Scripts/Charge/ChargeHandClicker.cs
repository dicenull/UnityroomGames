using UnityEngine;
using UnityEngine.EventSystems;

public class ChargeHandClicker : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"Clicked on {gameObject.name}");
        var data = GetIt.Instance.Get<ChargeGameData>();
        data.CurrentHand.Value = ChargeHandsExtend.ToChargeHands(gameObject.name);
    }
}
