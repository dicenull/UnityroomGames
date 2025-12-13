using UnityEngine;
using UnityEngine.EventSystems;

public class ChargeHandClicker : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"Clicked on {gameObject.name}");
        var data = GetIt.Instance.Get<ChargeGameData>();
        var hand = ChargeHandsExtend.ToChargeHands(gameObject.name);
        data.JudgeByHand(hand);
    }
}
