using R3;
using UnityEngine;
using UnityEngine.UI;

public class ChargeHandViewer : MonoBehaviour
{
    void Start()
    {
        var data = GetIt.Instance.Get<ChargeGameData>();
        data.CurrentHand.Subscribe(hand =>
        {
            var image = transform.GetComponent<Image>();
            image.sprite = Resources.Load<Sprite>($"Hands/{hand}");
        }).AddTo(this);
    }
}
