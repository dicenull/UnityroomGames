using UnityEngine;
using TMPro;
using System.Collections;

public class BattleManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text battleLogText;

    private GameData gameData;
    private bool isDefending = false;

    void Start()
    {
        gameData = GameData.Instance;
    }

    /// <summary>
    /// 難易度に応じた敵を生成
    /// </summary>
    public void SpawnEnemy(int difficulty)
    {
        difficulty = Mathf.Clamp(difficulty, 0, 2);
        
        string enemyWord = EnemyData.GetRandomEnemy(difficulty);
        gameData.CurrentEnemy.Value = enemyWord;
        gameData.EnemyMaxHP.Value = EnemyData.CalculateEnemyHP(enemyWord);
        gameData.EnemyHP.Value = gameData.EnemyMaxHP.Value;
        gameData.EnemyAttack.Value = EnemyData.CalculateEnemyAttack(enemyWord);
        
        GenerateEnemyNextAction();
        AddBattleLog($"A wild {enemyWord} appeared! (HP: {gameData.EnemyHP.Value})");
    }

    /// <summary>
    /// 敵の次の行動を生成して予告
    /// </summary>
    private void GenerateEnemyNextAction()
    {
        gameData.EnemyNextAction.Value = $"攻撃: {gameData.EnemyAttack.Value}ダメージ";
    }

    /// <summary>
    /// プレイヤーの攻撃
    /// </summary>
    public void PlayerAttack()
    {
        if (!gameData.IsPlayerTurn.Value) return;

        int attackPower = CharacterStats.CalculateAttackPower(gameData.Weapon.Value);
        gameData.EnemyHP.Value -= attackPower;
        
        AddBattleLog($"あなたは {attackPower} ダメージを与えた!");

        if (gameData.EnemyHP.Value <= 0)
        {
            CheckVictory();
        }
        else
        {
            StartCoroutine(EnemyTurnCoroutine());
        }
    }

    /// <summary>
    /// プレイヤーの防御
    /// </summary>
    public void PlayerDefend()
    {
        if (!gameData.IsPlayerTurn.Value) return;

        isDefending = true;
        AddBattleLog("あなたは防御態勢を取った!");
        
        StartCoroutine(EnemyTurnCoroutine());
    }

    /// <summary>
    /// 敵のターン（コルーチンで少し遅延）
    /// </summary>
    private IEnumerator EnemyTurnCoroutine()
    {
        gameData.IsPlayerTurn.Value = false;
        yield return new WaitForSeconds(0.5f);
        
        EnemyTurn();
        
        yield return new WaitForSeconds(0.5f);
        gameData.IsPlayerTurn.Value = true;
    }

    /// <summary>
    /// 敵のターン
    /// </summary>
    private void EnemyTurn()
    {
        int damage = gameData.EnemyAttack.Value;

        if (isDefending)
        {
            int defensePower = CharacterStats.CalculateDefensePower(gameData.Shield.Value);
            damage = Mathf.Max(0, damage - defensePower);
            AddBattleLog($"{gameData.CurrentEnemy.Value}の攻撃! 盾で {defensePower} 軽減!");
            isDefending = false;
        }
        else
        {
            AddBattleLog($"{gameData.CurrentEnemy.Value}の攻撃!");
        }

        gameData.PlayerHP.Value -= damage;
        AddBattleLog($"あなたは {damage} ダメージを受けた!");

        if (gameData.PlayerHP.Value <= 0)
        {
            CheckDefeat();
        }
        else
        {
            GenerateEnemyNextAction();
        }
    }

    /// <summary>
    /// 勝利判定
    /// </summary>
    private void CheckVictory()
    {
        gameData.DefeatedEnemies.Value++;
        AddBattleLog($"{gameData.CurrentEnemy.Value}を倒した!");
        
        // Phase 2では単純に次の敵を生成（Phase 3で報酬選択に変更）
        int nextDifficulty = gameData.DefeatedEnemies.Value / 2;
        StartCoroutine(SpawnNextEnemyCoroutine(nextDifficulty));
    }

    /// <summary>
    /// 次の敵生成（少し遅延）
    /// </summary>
    private IEnumerator SpawnNextEnemyCoroutine(int difficulty)
    {
        yield return new WaitForSeconds(1.5f);
        SpawnEnemy(difficulty);
    }

    /// <summary>
    /// 敗北判定
    /// </summary>
    private void CheckDefeat()
    {
        AddBattleLog("あなたは倒れた...");
        gameData.GameOver();
    }

    /// <summary>
    /// バトルログに追加
    /// </summary>
    private void AddBattleLog(string message)
    {
        if (battleLogText != null)
        {
            battleLogText.text = message + "\n" + battleLogText.text;
            
            // ログが長くなりすぎないように制限
            string[] lines = battleLogText.text.Split('\n');
            if (lines.Length > 10)
            {
                System.Array.Resize(ref lines, 10);
                battleLogText.text = string.Join("\n", lines);
            }
        }
    }
}
