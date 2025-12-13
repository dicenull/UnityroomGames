using R3;
using TMPro;
using UnityEngine;

public class CoolDownPreview : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coolDownText;

    void Start()
    {
        var gameData = GetIt.Instance.Get<InvGameData>();
        gameData.ShotCoolDown.Subscribe(coolDown =>
        {
            coolDownText.text = coolDown.ToString("F2");
        }).AddTo(this);
    }
}
