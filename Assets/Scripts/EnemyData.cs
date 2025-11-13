using System.Collections.Generic;
using UnityEngine;

public static class EnemyData
{
    // 難易度別の敵リスト
    public static readonly string[] EasyEnemies = { "CAT", "DOG", "RAT", "BAT" };
    public static readonly string[] MediumEnemies = { "BIRD", "FISH", "BEAR", "WOLF" };
    public static readonly string[] HardEnemies = { "TIGER", "EAGLE", "SHARK" };

    /// <summary>
    /// 敵のHPを計算（文字数 * 5）
    /// </summary>
    public static int CalculateEnemyHP(string word)
    {
        if (string.IsNullOrEmpty(word)) return 0;
        return word.Length * 5;
    }

    /// <summary>
    /// 敵の攻撃力を計算（文字数 + 2）
    /// </summary>
    public static int CalculateEnemyAttack(string word)
    {
        if (string.IsNullOrEmpty(word)) return 0;
        return word.Length + 2;
    }

    /// <summary>
    /// 難易度に応じてランダムな敵を取得
    /// </summary>
    /// <param name="difficulty">0=Easy, 1=Medium, 2=Hard</param>
    public static string GetRandomEnemy(int difficulty)
    {
        string[] enemies;

        switch (difficulty)
        {
            case 0:
                enemies = EasyEnemies;
                break;
            case 1:
                enemies = MediumEnemies;
                break;
            case 2:
                enemies = HardEnemies;
                break;
            default:
                enemies = EasyEnemies;
                break;
        }

        int randomIndex = Random.Range(0, enemies.Length);
        return enemies[randomIndex];
    }
}
