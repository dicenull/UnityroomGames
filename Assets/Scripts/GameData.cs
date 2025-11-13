
using R3;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public ReactiveProperty<bool> IsGameOver = new(false);
    
    // Player Data
    public ReactiveProperty<string> PlayerName = new("");
    public ReactiveProperty<string> Weapon = new("");
    public ReactiveProperty<string> Shield = new("");
    public ReactiveProperty<int> PotionCount = new(0);
    public ReactiveProperty<int> PlayerHP = new(20);
    public ReactiveProperty<int> PlayerMaxHP = new(20);

    // Enemy Data (Phase 2)
    public ReactiveProperty<string> CurrentEnemy = new("");
    public ReactiveProperty<int> EnemyHP = new(0);
    public ReactiveProperty<int> EnemyMaxHP = new(0);
    public ReactiveProperty<int> EnemyAttack = new(0);
    public ReactiveProperty<string> EnemyNextAction = new("");
    public ReactiveProperty<bool> IsPlayerTurn = new(true);
    public ReactiveProperty<int> DefeatedEnemies = new(0);
    
    public static GameData Instance { get; private set; }

    GameData()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void GameOver()
    {
        IsGameOver.Value = true;
    }

    public void InitializePlayer(string name)
    {
        if (string.IsNullOrEmpty(name))
            return;

        PlayerName.Value = name;
        Weapon.Value = name.Length >= 1 ? name[0].ToString() : "";
        Shield.Value = name.Length >= 2 ? name[name.Length - 1].ToString() : "";
        PotionCount.Value = Mathf.Max(0, name.Length - 2);
        PlayerHP.Value = PlayerMaxHP.Value;
    }

    public void Reset()
    {
        IsGameOver.Value = false;
        PlayerName.Value = "";
        Weapon.Value = "";
        Shield.Value = "";
        PotionCount.Value = 0;
        PlayerHP.Value = PlayerMaxHP.Value;

        // Enemy Data
        CurrentEnemy.Value = "";
        EnemyHP.Value = 0;
        EnemyMaxHP.Value = 0;
        EnemyAttack.Value = 0;
        EnemyNextAction.Value = "";
        IsPlayerTurn.Value = true;
        DefeatedEnemies.Value = 0;
    }
}
