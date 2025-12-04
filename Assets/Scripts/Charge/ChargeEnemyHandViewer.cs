using R3;
using UnityEngine;
using UnityEngine.UI;

public class ChargeEnemyHandViewer : MonoBehaviour
{
    Image image;
    void Awake()
    {
        image = transform.GetComponent<Image>();

        transform.Rotate(0, 0, 180);
    }

    void Start()
    {
        var data = GetIt.Instance.Get<ChargeGameData>();
        data.EnemyHand.Subscribe(hand =>
        {
            if (hand == null)
            {
                return;
            }

            image.sprite = Resources.Load<Sprite>($"Hands/{hand}");
        }).AddTo(this);
    }
}
