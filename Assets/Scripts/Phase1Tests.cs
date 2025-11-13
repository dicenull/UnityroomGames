using UnityEngine;

/// <summary>
/// Phase 1の統合テストを実行
/// </summary>
public class Phase1Tests : MonoBehaviour
{
    [ContextMenu("Run Phase 1 Tests")]
    public void RunAllTests()
    {
        Debug.Log("=== Phase 1 統合テスト開始 ===\n");

        TestCharacterStats();
        TestGameDataInitialization();

        Debug.Log("\n=== Phase 1 統合テスト完了 ===");
    }

    void TestCharacterStats()
    {
        Debug.Log("--- CharacterStats テスト ---");

        // Test 1: 単一文字
        int cAttack = CharacterStats.CalculateAttackPower("C");
        Debug.Log($"Test 1-1: 'C' 攻撃力 = {cAttack} (期待値: 3) {(cAttack == 3 ? "✅" : "❌")}");

        // Test 2: CAT
        int catAttack = CharacterStats.CalculateAttackPower("CAT");
        Debug.Log($"Test 1-2: 'CAT' 攻撃力 = {catAttack} (期待値: 8 = C:3 + A:2 + T:3) {(catAttack == 8 ? "✅" : "❌")}");

        // Test 3: 防御力
        int tDefense = CharacterStats.CalculateDefensePower("T");
        Debug.Log($"Test 1-3: 'T' 防御力 = {tDefense} (期待値: 2) {(tDefense == 2 ? "✅" : "❌")}");

        // Test 4: 母音
        int aAttack = CharacterStats.CalculateAttackPower("A");
        Debug.Log($"Test 1-4: 'A' (母音) 攻撃力 = {aAttack} (期待値: 2) {(aAttack == 2 ? "✅" : "❌")}");

        // Test 5: レア文字
        int xAttack = CharacterStats.CalculateAttackPower("X");
        Debug.Log($"Test 1-5: 'X' (レア) 攻撃力 = {xAttack} (期待値: 4) {(xAttack == 4 ? "✅" : "❌")}");

        // Test 6: 小文字
        int catLower = CharacterStats.CalculateAttackPower("cat");
        Debug.Log($"Test 1-6: 'cat' (小文字) 攻撃力 = {catLower} (期待値: 8) {(catLower == 8 ? "✅" : "❌")}");

        Debug.Log("");
    }

    void TestGameDataInitialization()
    {
        Debug.Log("--- GameData.InitializePlayer テスト ---");

        GameData gameData = GameData.Instance;
        if (gameData == null)
        {
            Debug.LogError("❌ GameData.Instance が null です。シーンに GameData オブジェクトを配置してください。");
            return;
        }

        // Test 1: "CAT"
        gameData.InitializePlayer("CAT");
        bool test1 = gameData.Weapon.Value == "C" && 
                     gameData.Shield.Value == "T" && 
                     gameData.PotionCount.Value == 1;
        Debug.Log($"Test 2-1: 'CAT' → 武器:{gameData.Weapon.Value}, 盾:{gameData.Shield.Value}, ポーション:{gameData.PotionCount.Value}");
        Debug.Log($"  期待値: 武器:C, 盾:T, ポーション:1 {(test1 ? "✅" : "❌")}");

        // 武器と盾のステータス表示
        int catWeaponAtk = CharacterStats.CalculateAttackPower(gameData.Weapon.Value);
        int catShieldDef = CharacterStats.CalculateDefensePower(gameData.Shield.Value);
        Debug.Log($"  武器'C' ATK:{catWeaponAtk} (期待値:3), 盾'T' DEF:{catShieldDef} (期待値:2)");

        // Test 2: "AT"
        gameData.InitializePlayer("AT");
        bool test2 = gameData.Weapon.Value == "A" && 
                     gameData.Shield.Value == "T" && 
                     gameData.PotionCount.Value == 0;
        Debug.Log($"Test 2-2: 'AT' → 武器:{gameData.Weapon.Value}, 盾:{gameData.Shield.Value}, ポーション:{gameData.PotionCount.Value}");
        Debug.Log($"  期待値: 武器:A, 盾:T, ポーション:0 {(test2 ? "✅" : "❌")}");

        int atWeaponAtk = CharacterStats.CalculateAttackPower(gameData.Weapon.Value);
        int atShieldDef = CharacterStats.CalculateDefensePower(gameData.Shield.Value);
        Debug.Log($"  武器'A' ATK:{atWeaponAtk} (期待値:2), 盾'T' DEF:{atShieldDef} (期待値:2)");

        // Test 3: "X"
        gameData.InitializePlayer("X");
        bool test3 = gameData.Weapon.Value == "X" && 
                     gameData.Shield.Value == "" && 
                     gameData.PotionCount.Value == 0;
        Debug.Log($"Test 2-3: 'X' → 武器:{gameData.Weapon.Value}, 盾:{gameData.Shield.Value}, ポーション:{gameData.PotionCount.Value}");
        Debug.Log($"  期待値: 武器:X, 盾:(なし), ポーション:0 {(test3 ? "✅" : "❌")}");

        int xWeaponAtk = CharacterStats.CalculateAttackPower(gameData.Weapon.Value);
        Debug.Log($"  武器'X' ATK:{xWeaponAtk} (期待値:4)");

        // Test 4: HP初期化
        bool test4 = gameData.PlayerHP.Value == 20 && gameData.PlayerMaxHP.Value == 20;
        Debug.Log($"Test 2-4: HP初期値 → HP:{gameData.PlayerHP.Value}/{gameData.PlayerMaxHP.Value}");
        Debug.Log($"  期待値: 20/20 {(test4 ? "✅" : "❌")}");

        // Test 5: Reset
        gameData.Reset();
        bool test5 = gameData.PlayerName.Value == "" && 
                     gameData.Weapon.Value == "" && 
                     gameData.Shield.Value == "";
        Debug.Log($"Test 2-5: Reset() → 名前:'{gameData.PlayerName.Value}', 武器:'{gameData.Weapon.Value}', 盾:'{gameData.Shield.Value}'");
        Debug.Log($"  期待値: すべて空文字 {(test5 ? "✅" : "❌")}");

        Debug.Log("");
    }
}
