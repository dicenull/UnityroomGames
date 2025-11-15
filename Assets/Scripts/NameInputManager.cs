using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class NameInputManager : MonoBehaviour
{
    [Header("Name Input Panel")]
    [SerializeField] private GameObject nameInputPanel;
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private TextMeshProUGUI errorText;

    [Header("Equipment Display Panel")]
    [SerializeField] private GameObject equipmentDisplayPanel;
    [SerializeField] private TextMeshProUGUI weaponText;
    [SerializeField] private TextMeshProUGUI shieldText;
    [SerializeField] private TextMeshProUGUI potionText;

    [Header("Battle Manager")]
    [SerializeField] private BattleManager battleManager;

    private void Start()
    {
        nameInputPanel.SetActive(true);
        equipmentDisplayPanel.SetActive(false);

        if (errorText != null)
            errorText.gameObject.SetActive(false);
    }

    public void OnStartGameClicked()
    {
        string playerName = nameInputField.text.Trim();

        if (string.IsNullOrEmpty(playerName))
        {
            ShowError("Please enter your name!");
            return;
        }

        if (!IsValidName(playerName))
        {
            ShowError("Only English letters are allowed!");
            return;
        }

        GameData.Instance.InitializePlayer(playerName);
        ShowEquipmentDisplay();
    }

    private bool IsValidName(string name)
    {
        return Regex.IsMatch(name, @"^[a-zA-Z]+$");
    }

    private void ShowError(string message)
    {
        if (errorText != null)
        {
            errorText.text = message;
            errorText.gameObject.SetActive(true);
        }
    }

    private void ShowEquipmentDisplay()
    {
        nameInputPanel.SetActive(false);
        equipmentDisplayPanel.SetActive(true);

        string weapon = GameData.Instance.Weapon.Value;
        string shield = GameData.Instance.Shield.Value;
        int potionCount = GameData.Instance.PotionCount.Value;

        int weaponAttack = CharacterStats.CalculateAttackPower(weapon);
        int shieldDefense = CharacterStats.CalculateDefensePower(shield);

        weaponText.text = string.IsNullOrEmpty(weapon)
            ? "/"
            : $"{weapon} ({weaponAttack})".ToUpper();

        shieldText.text = string.IsNullOrEmpty(shield)
            ? "/"
            : $"{shield} ({shieldDefense})".ToUpper();

        potionText.text = $"{potionCount}";
    }

    public void OnBeginBattleClicked()
    {
        equipmentDisplayPanel.SetActive(false);

        if (battleManager != null)
        {
            battleManager.StartBattle();
        }
        else
        {
            Debug.LogError("BattleManager is not assigned in NameInputManager!");
        }
    }
}
