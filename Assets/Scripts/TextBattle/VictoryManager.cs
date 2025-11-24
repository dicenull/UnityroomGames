using UnityEngine;
using TMPro;

/// <summary>
/// Phase 5: ゲームクリア画面の管理
/// </summary>
public class VictoryManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI statsText;
    [SerializeField] private OperationPanel operationPanel;

    private void Update()
    {
        if (victoryPanel == null || !victoryPanel.activeSelf) return;

        // Enterキーでリスタート
        var keyboard = UnityEngine.InputSystem.Keyboard.current;
        if (keyboard != null && keyboard[UnityEngine.InputSystem.Key.Enter].wasPressedThisFrame)
        {
            RestartGame();
        }
    }

    /// <summary>
    /// クリア画面を表示
    /// </summary>
    public void ShowVictoryScreen()
    {
        if (victoryPanel == null) return;

        victoryPanel.SetActive(true);

        if (titleText != null)
        {
            titleText.text = "=== VICTORY ===";
        }

        if (statsText != null)
        {
            int defeatedEnemies = GameData.Instance.DefeatedEnemies.Value;
            string weapon = GameData.Instance.Weapon.Value;
            string shield = GameData.Instance.Shield.Value;
            int weaponAtk = CharacterStats.CalculateAttackPower(weapon);
            int shieldDef = CharacterStats.CalculateDefensePower(shield);

            statsText.text = $"Enemies Defeated: {defeatedEnemies}\n\n" +
                           $"Final Equipment:\n" +
                           $"Weapon: {weapon} (ATK: {weaponAtk})\n" +
                           $"Shield: {shield} (DEF: {shieldDef})";
        }

        // OperationPanelにリスタート操作を表示
        if (operationPanel != null)
        {
            var operationText = operationPanel.GetComponent<TextMeshProUGUI>();
            if (operationText == null)
            {
                operationText = operationPanel.GetComponentInChildren<TextMeshProUGUI>();
            }
            if (operationText != null)
            {
                operationText.text = "Restart [Enter]";
            }
        }
    }

    /// <summary>
    /// ゲームをリスタート
    /// </summary>
    private void RestartGame()
    {
        GameData.Instance.Reset();
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }
}
