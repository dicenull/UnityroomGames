using R3;
using TMPro;
using UnityEngine;

public class ChargeCountViewer : MonoBehaviour
{

    void Start()
    {
        var data = GetIt.Instance.Get<ChargeGameData>();
        data.PlayerCharge.Subscribe(count =>
        {
            var text = transform.GetComponent<TextMeshProUGUI>();
            text.text = $"{count}";
        }).AddTo(this);
    }
}
