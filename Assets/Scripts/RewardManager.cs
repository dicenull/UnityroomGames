using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

/// <summary>
/// Phase 3: 敵撃破時の報酬選択システム
/// 敵の文字から1文字を選択し、武器または盾に追加する
/// </summary>
public class RewardManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject rewardPanel;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Transform letterButtonContainer;
    [SerializeField] private GameObject letterButtonPrefab;
    [SerializeField] private GameObject equipChoicePanel;
    [SerializeField] private TextMeshProUGUI equipChoiceText;
    [SerializeField] private GameObject weaponKeyIndicator;
    [SerializeField] private GameObject shieldKeyIndicator;

    private BattleManager battleManager;
    private char selectedLetter;

    private void Awake()
    {
        battleManager = FindObjectOfType<BattleManager>();
    }

    private void Update()
    {
        // キーボード入力で装備選択（装備選択画面が表示されている時のみ）
        if (equipChoicePanel != null && equipChoicePanel.activeSelf)
        {
            var keyboard = UnityEngine.InputSystem.Keyboard.current;
            if (keyboard == null) return;

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

        // 既存の文字ボタンをクリア
        foreach (Transform child in letterButtonContainer)
        {
            Destroy(child.gameObject);
        }

        // 敵の文字ごとにボタンを生成
        foreach (char letter in enemyWord)
        {
            GameObject buttonObj = Instantiate(letterButtonPrefab, letterButtonContainer);
            TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = letter.ToString();
            }

            Button button = buttonObj.GetComponent<Button>();
            if (button != null)
            {
                char capturedLetter = letter; // クロージャのためキャプチャ
                button.onClick.AddListener(() => OnLetterSelected(capturedLetter));
            }
        }

        rewardPanel.SetActive(true);
        if (equipChoicePanel != null)
            equipChoicePanel.SetActive(false);
        
        if (titleText != null)
            titleText.text = $"Enemy '{enemyWord}' defeated! Choose a letter:";
    }

    /// <summary>
    /// 文字が選択されたときの処理
    /// </summary>
    private void OnLetterSelected(char letter)
    {
        selectedLetter = letter;

        if (equipChoicePanel != null)
        {
            equipChoicePanel.SetActive(true);
        }

        if (equipChoiceText != null)
        {
            string weaponKey = GameData.Instance.Weapon.Value;
            string shieldKey = GameData.Instance.Shield.Value;
            equipChoiceText.text = $"Add '{letter}' to...
Weapon [{weaponKey}] / Shield [{shieldKey}]";
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
        if (equipChoicePanel != null)
            equipChoicePanel.SetActive(false);
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
