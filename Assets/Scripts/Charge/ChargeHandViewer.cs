using R3;
using UnityEngine;
using UnityEngine.UI;

public class ChargeHandViewer : MonoBehaviour
{
    Image image;
    void Awake()
    {
        image = transform.GetComponent<Image>();
    }

    void Start()
    {
        var data = GetIt.Instance.Get<ChargeGameData>();
        data.CurrentHand.Subscribe(hand =>
        {
            if (hand == null)
            {
                return;
            }

            image.sprite = Resources.Load<Sprite>($"Hands/{hand}");
        }).AddTo(this);
    }
}
