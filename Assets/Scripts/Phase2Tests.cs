using UnityEngine;

/// <summary>
/// Phase 2の統合テストを実行
/// </summary>
public class Phase2Tests : MonoBehaviour
{
    [ContextMenu("Run Phase 2 Tests")]
    public void RunAllTests()
    {
        Debug.Log("=== Phase 2 統合テスト開始 ===\n");

        TestEnemyData();
        TestGameDataBattleState();

        Debug.Log("\n=== Phase 2 統合テスト完了 ===");
    }

    void TestEnemyData()
    {
        Debug.Log("--- EnemyData テスト ---");

        // Test 1: HP計算
        int catHP = EnemyData.CalculateEnemyHP("CAT");
        Debug.Log($"Test 1-1: 'CAT' HP = {catHP} (期待値: 15 = 3文字 * 5) {(catHP == 15 ? "✅" : "❌")}");

        // Test 2: 攻撃力計算
        int catAttack = EnemyData.CalculateEnemyAttack("CAT");
        Debug.Log($"Test 1-2: 'CAT' 攻撃力 = {catAttack} (期待値: 5 = 3文字 + 2) {(catAttack == 5 ? "✅" : "❌")}");

        // Test 3: 難易度別の敵取得
        string easyEnemy = EnemyData.GetRandomEnemy(0);
        bool isEasy = System.Array.IndexOf(EnemyData.EasyEnemies, easyEnemy) >= 0;
        Debug.Log($"Test 1-3: Easy敵 = '{easyEnemy}' {(isEasy ? "✅" : "❌")}");

        string mediumEnemy = EnemyData.GetRandomEnemy(1);
        bool isMedium = System.Array.IndexOf(EnemyData.MediumEnemies, mediumEnemy) >= 0;
        Debug.Log($"Test 1-4: Medium敵 = '{mediumEnemy}' {(isMedium ? "✅" : "❌")}");

        string hardEnemy = EnemyData.GetRandomEnemy(2);
        bool isHard = System.Array.IndexOf(EnemyData.HardEnemies, hardEnemy) >= 0;
        Debug.Log($"Test 1-5: Hard敵 = '{hardEnemy}' {(isHard ? "✅" : "❌")}");

        // Test 4: より長い敵
        int tigerHP = EnemyData.CalculateEnemyHP("TIGER");
        int tigerAttack = EnemyData.CalculateEnemyAttack("TIGER");
        Debug.Log($"Test 1-6: 'TIGER' HP={tigerHP} (期待値:25), ATK={tigerAttack} (期待値:7)");
        Debug.Log($"  {(tigerHP == 25 && tigerAttack == 7 ? "✅" : "❌")}");

        Debug.Log("");
    }

    void TestGameDataBattleState()
    {
        Debug.Log("--- GameData 戦闘状態管理テスト ---");

        GameData gameData = GameData.Instance;
        if (gameData == null)
        {
            Debug.LogError("❌ GameData.Instance が null です。");
            return;
        }

        // Test 1: 初期状態
        bool test1 = gameData.CurrentEnemy.Value == "" &&
                     gameData.EnemyHP.Value == 0 &&
                     gameData.IsPlayerTurn.Value == true &&
                     gameData.DefeatedEnemies.Value == 0;
        Debug.Log($"Test 2-1: 初期状態 {(test1 ? "✅" : "❌")}");
        Debug.Log($"  Enemy:'{gameData.CurrentEnemy.Value}', HP:{gameData.EnemyHP.Value}, PlayerTurn:{gameData.IsPlayerTurn.Value}, Defeated:{gameData.DefeatedEnemies.Value}");

        // Test 2: 敵の状態設定をシミュレート
        gameData.CurrentEnemy.Value = "CAT";
        gameData.EnemyMaxHP.Value = 15;
        gameData.EnemyHP.Value = 15;
        gameData.EnemyAttack.Value = 5;
        gameData.EnemyNextAction.Value = "攻撃: 5ダメージ";

        bool test2 = gameData.CurrentEnemy.Value == "CAT" &&
                     gameData.EnemyHP.Value == 15 &&
                     gameData.EnemyAttack.Value == 5;
        Debug.Log($"Test 2-2: 敵状態設定 {(test2 ? "✅" : "❌")}");
        Debug.Log($"  Enemy:{gameData.CurrentEnemy.Value}, HP:{gameData.EnemyHP.Value}/{gameData.EnemyMaxHP.Value}, ATK:{gameData.EnemyAttack.Value}");
        Debug.Log($"  Next Action:{gameData.EnemyNextAction.Value}");

        // Test 3: ターン切り替え
        gameData.IsPlayerTurn.Value = false;
        Debug.Log($"Test 2-3: ターン切り替え (Player→Enemy) {(!gameData.IsPlayerTurn.Value ? "✅" : "❌")}");

        gameData.IsPlayerTurn.Value = true;
        Debug.Log($"Test 2-4: ターン切り替え (Enemy→Player) {(gameData.IsPlayerTurn.Value ? "✅" : "❌")}");

        // Test 4: 撃破数カウント
        gameData.DefeatedEnemies.Value = 3;
        Debug.Log($"Test 2-5: 撃破数 = {gameData.DefeatedEnemies.Value} (期待値:3) {(gameData.DefeatedEnemies.Value == 3 ? "✅" : "❌")}");

        // Test 5: Reset
        gameData.Reset();
        bool test5 = gameData.CurrentEnemy.Value == "" &&
                     gameData.EnemyHP.Value == 0 &&
                     gameData.DefeatedEnemies.Value == 0 &&
                     gameData.IsPlayerTurn.Value == true;
        Debug.Log($"Test 2-6: Reset() {(test5 ? "✅" : "❌")}");

        Debug.Log("");
    }
}
