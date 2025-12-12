using UnityEngine;

public class InvDiceController : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        var gameData = GetIt.Instance.Get<InvGameData>();
        InvPhase? nextPhase = other.name switch
        {
            "RightBar" => InvPhase.Left,
            "LeftBar" => InvPhase.Right,
            _ => null
        };
        if (nextPhase == null)
        {
            CheckBullet(other.gameObject);
            return;
        }

        gameData.Hit(nextPhase.Value);
    }

    void CheckBullet(GameObject other)
    {
        if (!other.name.Contains("InvBullet"))
        {
            return;
        }

        Destroy(other);
        Destroy(gameObject);
    }
}
