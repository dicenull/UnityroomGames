using R3;
using TMPro;
using UnityEngine;

public class ChargeResultViewer : MonoBehaviour
{
    void Start()
    {
        var data = GetIt.Instance.Get<ChargeGameData>();
        data.IsWin.Subscribe(result =>
        {
            var text = transform.GetComponent<TextMeshProUGUI>();
            if (result == null)
            {
                return;
            }

            if (result == true)
            {
                text.text = "You Win!";
            }
            else
            {
                text.text = "You Lose...";
            }
        }).AddTo(this);
    }

}
