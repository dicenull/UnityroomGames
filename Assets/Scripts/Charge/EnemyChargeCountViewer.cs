using R3;
using TMPro;
using UnityEngine;

public class EnemyChargeCountViewer : MonoBehaviour
{

    void Start()
    {
        var data = GetIt.Instance.Get<ChargeGameData>();
        data.EnemyCharge.Subscribe(count =>
        {
            var text = transform.GetComponent<TextMeshProUGUI>();
            text.text = $"{count}";
        }).AddTo(this);
    }
}
