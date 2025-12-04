using R3;
using UnityEngine;

public class ChargeJudge : MonoBehaviour
{
    void Start()
    {
        var data = GetIt.Instance.Get<ChargeGameData>();
        // CurrentHandかEnemyHandが変化したら勝敗を判定してログに出す
        Observable.Merge(data.CurrentHand, data.EnemyHand).Subscribe(_ =>
        {
            var playerHand = data.CurrentHand.Value;
            var enemyHand = data.EnemyHand.Value;
            if (playerHand == null || enemyHand == null)
            {
                return;
            }

            var pCharge = data.PlayerCharge.Value;
            var eCharge = data.EnemyCharge.Value;
            Debug.Log($"PlayerHand: {playerHand}, EnemyHand: {enemyHand}, PlayerCharge: {pCharge}, EnemyCharge: {eCharge}");
            var result = JudgeManager.Judge(playerHand.Value, pCharge, enemyHand.Value, eCharge);

            Debug.Log($"{result}");

        }).AddTo(this);
    }
}

static class JudgeManager
{

    public static JudgeResult Judge(ChargeHands player, int playerCharge, ChargeHands enemy, int enemyCharge)
    {
        var isPlayerInValidBeam = player == ChargeHands.Beam && playerCharge <= 0;
        if (isPlayerInValidBeam)
        {
            return JudgeResult.Lose;
        }
        var isEnemyInValidBeam = enemy == ChargeHands.Beam && enemyCharge <= 0;
        if (isEnemyInValidBeam)
        {
            return JudgeResult.Win;
        }
        if (player == ChargeHands.Charge && enemy == ChargeHands.Beam)
        {
            return JudgeResult.Lose;
        }
        if (player == ChargeHands.Beam && enemy == ChargeHands.Charge)
        {
            return JudgeResult.Win;
        }

        return JudgeResult.Draw;
    }
}
public enum JudgeResult
{
    Win,
    Lose,
    Draw
}