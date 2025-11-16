using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

/// <summary>
/// Phase 3: 敵撃破時の報酬選択システム
/// 敵の文字から1文字を選択し、武器または盾に追加する
/// </summary>
/// <summary>
/// Phase 3: 敵撃破時の報酬選択システム
/// FR-021対応: キーボード入力のみで操作（ボタン不使用）
/// </summary>
/// <summary>
/// Phase 3: 敵撃破時の報酬選択システム
/// FR-021対応: キーボード入力のみで操作（ボタン不使用）
/// </summary>
public class RewardManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject rewardPanel;
    [SerializeField] private OperationPanel operationPanel;

    private BattleManager battleManager;
    private char selectedLetter;
    private string currentEnemyWord;
    private bool isEquipSelectionPhase = false;

    private void Awake()
    {
        battleManager = FindObjectOfType<BattleManager>();
        if (operationPanel == null)
        {
            operationPanel = FindObjectOfType<OperationPanel>();
        }
    }

    private void Update()
    {
        if (rewardPanel == null || !rewardPanel.activeSelf) return;

        var keyboard = UnityEngine.InputSystem.Keyboard.current;
        if (keyboard == null) return;

        // 文字選択フェーズ
        if (!isEquipSelectionPhase && !string.IsNullOrEmpty(currentEnemyWord))
        {
            // 数字キー1～9で文字選択
            for (int i = 0; i < currentEnemyWord.Length && i < 9; i++)
            {
                var key = UnityEngine.InputSystem.Key.Digit1 + i;
                if (keyboard[key].wasPressedThisFrame)
                {
                    OnLetterSelected(currentEnemyWord[i]);
                    break;
                }
            }
        }
        // 装備選択フェーズ
        else if (isEquipSelectionPhase)
        {
            // 武器キー
            string weaponKey = GameData.Instance.Weapon.Value;
            if (!string.IsNullOrEmpty(weaponKey))
            {
                var key = StringToKey.FromString(weaponKey);
                if (keyboard[key].wasPressedThisFrame)
                {
                    AddToWeapon();
                }
            }

            // 盾キー
            string shieldKey = GameData.Instance.Shield.Value;
            if (!string.IsNullOrEmpty(shieldKey))
            {
                var key = StringToKey.FromString(shieldKey);
                if (keyboard[key].wasPressedThisFrame)
                {
                    AddToShield();
                }
            }
        }
    }

    /// <summary>
    /// 敵撃破時に報酬選択画面を表示
    /// </summary>
    public void ShowRewardPanel(string enemyWord)
    {
        if (rewardPanel == null) return;

        currentEnemyWord = enemyWord;
        isEquipSelectionPhase = false;

        rewardPanel.SetActive(true);
        
        // OperationPanelに文字選択操作を表示
        if (operationPanel != null)
        {
            operationPanel.ShowRewardLetterSelection(enemyWord);
        }
    }

    /// <summary>
    /// 文字が選択されたときの処理
    /// </summary>
    private void OnLetterSelected(char letter)
    {
        selectedLetter = letter;
        isEquipSelectionPhase = true;

        // OperationPanelに装備選択操作を表示
        if (operationPanel != null)
        {
            operationPanel.ShowRewardEquipSelection(letter);
        }
    }

    /// <summary>
    /// 武器に文字を追加
    /// </summary>
    private void AddToWeapon()
    {
        GameData.Instance.AddToWeapon(selectedLetter);
        CloseRewardPanel();
        SpawnNextEnemy();
    }

    /// <summary>
    /// 盾に文字を追加
    /// </summary>
    private void AddToShield()
    {
        GameData.Instance.AddToShield(selectedLetter);
        CloseRewardPanel();
        SpawnNextEnemy();
    }

    /// <summary>
    /// 報酬画面を閉じる
    /// </summary>
    private void CloseRewardPanel()
    {
        if (rewardPanel != null)
            rewardPanel.SetActive(false);
        
        currentEnemyWord = "";
        isEquipSelectionPhase = false;

        // OperationPanelを戦闘操作表示に戻す
        if (operationPanel != null)
        {
            operationPanel.ShowBattleOperations();
        }
    }

    /// <summary>
    /// 次の敵を生成（難易度スケーリング付き）
    /// </summary>
    private void SpawnNextEnemy()
    {
        int defeatedCount = GameData.Instance.DefeatedEnemies.Value;
        int difficulty = Mathf.Clamp(defeatedCount / 2, 0, 2); // 2体ごとに難易度上昇
        
        if (battleManager != null)
        {
            battleManager.SpawnEnemy(difficulty);
        }
    }
}
