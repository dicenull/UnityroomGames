using UnityEngine;
using TMPro;
using R3;
using System;

/// <summary>
/// FR-020: 現在選択可能な操作の一覧を常時表示する専用UIパネル
/// ターン状態に応じて動的に更新
/// </summary>
public class OperationPanel : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI operationText;

    private IDisposable weaponSubscription;
    private IDisposable shieldSubscription;
    private IDisposable potionSubscription;
    private IDisposable gameStateSubscription;

    private void Start()
    {
        UpdateOperationText();
        SubscribeToGameData();
    }

    private void SubscribeToGameData()
    {
        // 武器・盾・ポーション数・敵情報の変更を監視
        weaponSubscription = GameData.Instance.Weapon.Subscribe(_ => UpdateOperationText());
        shieldSubscription = GameData.Instance.Shield.Subscribe(_ => UpdateOperationText());
        potionSubscription = GameData.Instance.PotionCount.Subscribe(_ => UpdateOperationText());
        gameStateSubscription = GameData.Instance.CurrentEnemy.Subscribe(_ => UpdateOperationText());
    }

    private void UpdateOperationText()
    {
        if (operationText == null) return;

        string weapon = GameData.Instance.Weapon.Value;
        string shield = GameData.Instance.Shield.Value;
        int potionCount = GameData.Instance.PotionCount.Value;

        // 名前入力フェーズ
        if (string.IsNullOrEmpty(weapon))
        {
            operationText.text = "Start Game [Enter]";
            return;
        }

        // 装備確認フェーズ（戦闘開始前）
        if (GameData.Instance.CurrentEnemy.Value == "")
        {
            operationText.text = "Begin Battle [Enter]";
            return;
        }

        // 戦闘フェーズ
        string operations = "";
        operations += $"Attack [{weapon}] - Use your weapon\n";
        operations += $"Defend [{shield}] - Use your shield\n";

        if (potionCount > 0)
        {
            operations += $"Use Potion [R] - Heal HP ({potionCount} left)";
        }
        else
        {
            operations += "Use Potion [R] - <color=#888888>(No potions)</color>";
        }

        operationText.text = operations;
    }

    /// <summary>
    /// 報酬選択フェーズの操作表示（RewardManagerから呼び出される）
    /// </summary>
    public void ShowRewardLetterSelection(string enemyWord)
    {
        if (operationText == null) return;

        string operations = "Choose a letter:\n";
        for (int i = 0; i < enemyWord.Length && i < 9; i++)
        {
            operations += $"[{i + 1}] {enemyWord[i]}\n";
        }

        operationText.text = operations;
    }

    /// <summary>
    /// 装備選択フェーズの操作表示（RewardManagerから呼び出される）
    /// </summary>
    public void ShowRewardEquipSelection(char letter)
    {
        if (operationText == null) return;

        string weapon = GameData.Instance.Weapon.Value;
        string shield = GameData.Instance.Shield.Value;

        operationText.text = $"Add '{letter}' to:\nWeapon [{weapon}] / Shield [{shield}]";
    }

    /// <summary>
    /// 通常の戦闘操作表示に戻す（RewardManagerから呼び出される）
    /// </summary>
    public void ShowBattleOperations()
    {
        UpdateOperationText();
    }

    private void OnDestroy()
    {
        weaponSubscription?.Dispose();
        shieldSubscription?.Dispose();
        potionSubscription?.Dispose();
        gameStateSubscription?.Dispose();
    }
}
