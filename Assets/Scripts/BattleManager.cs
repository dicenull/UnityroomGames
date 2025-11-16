using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using R3;

public class BattleManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject battlePanel;
    [SerializeField] private RewardManager rewardManager;
    [SerializeField] private TMP_Text playerHPText;
    [SerializeField] private TMP_Text enemyInfoText;
    [SerializeField] private TMP_Text enemyNextActionText;
    [SerializeField] private TMP_Text playerEquipmentText;
    [SerializeField] private TMP_Text battleLogText;
    [SerializeField] private Button attackButton;
    [SerializeField] private Button defendButton;
    [SerializeField] private Button potionButton;

    private GameData gameData
    {
        get
        {
            return GameData.Instance;
        }
    }
    private bool isDefending = false;

    void Start()
    {
        SetupUIBindings();
    }

    /// <summary>
    /// ReactivePropertyとUIの連携を設定
    /// </summary>
    private void SetupUIBindings()
    {
        if (gameData == null) return;

        // プレイヤーHP表示
        gameData.PlayerHP.Subscribe(hp =>
        {
            if (playerHPText != null)
                playerHPText.text = $"HP: {hp}/{gameData.PlayerMaxHP.Value}";
        }).AddTo(this);

        // 敵情報表示
        gameData.CurrentEnemy.Subscribe(enemy =>
        {
            if (enemyInfoText != null && !string.IsNullOrEmpty(enemy))
                UpdateEnemyInfo();
        }).AddTo(this);

        gameData.EnemyHP.Subscribe(_ =>
        {
            if (enemyInfoText != null)
                UpdateEnemyInfo();
        }).AddTo(this);

        // 敵の次の行動表示
        gameData.EnemyNextAction.Subscribe(action =>
        {
            if (enemyNextActionText != null)
                enemyNextActionText.text = $"Next: {action}";
        }).AddTo(this);

        // 装備情報表示
        gameData.Weapon.Subscribe(_ => UpdateEquipmentInfo()).AddTo(this);
        gameData.Shield.Subscribe(_ => UpdateEquipmentInfo()).AddTo(this);

        // ターン管理によるボタン制御
        gameData.IsPlayerTurn.Subscribe(isPlayerTurn =>
        {
            if (attackButton != null) attackButton.interactable = isPlayerTurn;
            if (defendButton != null) defendButton.interactable = isPlayerTurn;
            if (potionButton != null) potionButton.interactable = isPlayerTurn && gameData.PotionCount.Value > 0;
        }).AddTo(this);

        // ポーション数によるボタン制御
        gameData.PotionCount.Subscribe(count =>
        {
            if (potionButton != null)
            {
                potionButton.interactable = gameData.IsPlayerTurn.Value && count > 0;
                var buttonText = potionButton.GetComponentInChildren<TMP_Text>();
                if (buttonText != null)
                    buttonText.text = $"Use Potion ({count})";
            }
        }).AddTo(this);
    }

    /// <summary>
    /// 敵情報を更新
    /// </summary>
    private void UpdateEnemyInfo()
    {
        if (enemyInfoText != null)
        {
            enemyInfoText.text = $"Enemy: {gameData.CurrentEnemy.Value} HP: {gameData.EnemyHP.Value}/{gameData.EnemyMaxHP.Value}";
        }
    }

    /// <summary>
    /// 装備情報を更新
    /// </summary>
    private void UpdateEquipmentInfo()
    {
        if (playerEquipmentText != null)
        {
            int atk = CharacterStats.CalculateAttackPower(gameData.Weapon.Value);
            int def = CharacterStats.CalculateDefensePower(gameData.Shield.Value);
            string weapon = string.IsNullOrEmpty(gameData.Weapon.Value) ? "None" : gameData.Weapon.Value;
            string shield = string.IsNullOrEmpty(gameData.Shield.Value) ? "None" : gameData.Shield.Value;
            playerEquipmentText.text = $"Weapon: {weapon} (ATK: {atk}) / Shield: {shield} (DEF: {def})";
        }
    }

    /// <summary>
    /// 戦闘パネルを表示して最初の敵を生成
    /// </summary>
    public void StartBattle()
    {
        if (battlePanel != null)
            battlePanel.SetActive(true);

        SpawnEnemy(0);
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
        gameData.EnemyNextAction.Value = $"Attack: {gameData.EnemyAttack.Value} DMG";
    }

    /// <summary>
    /// プレイヤーの攻撃
    /// </summary>
    public void PlayerAttack()
    {
        if (!gameData.IsPlayerTurn.Value) return;

        int attackPower = CharacterStats.CalculateAttackPower(gameData.Weapon.Value);
        gameData.EnemyHP.Value -= attackPower;

        AddBattleLog($"You dealt {attackPower} damage!");

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
        AddBattleLog("You defend!");

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
            AddBattleLog($"{gameData.CurrentEnemy.Value} attacks! Shield blocked {defensePower}!");
            isDefending = false;
        }
        else
        {
            AddBattleLog($"{gameData.CurrentEnemy.Value} attacks!");
        }

        gameData.PlayerHP.Value -= damage;
        AddBattleLog($"You took {damage} damage!");

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
        string defeatedEnemy = gameData.CurrentEnemy.Value;
        AddBattleLog($"Defeated {defeatedEnemy}!");

        // Phase 3: 報酬選択画面を表示
        if (rewardManager != null)
        {
            battlePanel.SetActive(false);
            rewardManager.ShowRewardPanel(defeatedEnemy);
        }
        else
        {
            // RewardManagerがない場合は従来通り
            int nextDifficulty = gameData.DefeatedEnemies.Value / 2;
            StartCoroutine(SpawnNextEnemyCoroutine(nextDifficulty));
        }
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
        AddBattleLog("You were defeated...");
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
