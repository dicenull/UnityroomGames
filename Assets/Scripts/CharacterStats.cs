using System.Collections.Generic;
using UnityEngine;

public static class CharacterStats
{
    private static readonly Dictionary<char, (int attack, int defense)> letterStats = new Dictionary<char, (int, int)>
    {
        // 母音 (A,E,I,O,U): 攻撃力2, 防御力3
        {'A', (2, 3)}, {'E', (2, 3)}, {'I', (2, 3)}, {'O', (2, 3)}, {'U', (2, 3)},
        
        // レア文字 (Q,X,Z): 攻撃力4, 防御力4
        {'Q', (4, 4)}, {'X', (4, 4)}, {'Z', (4, 4)},
        
        // 通常子音: 攻撃力3, 防御力2
        {'B', (3, 2)}, {'C', (3, 2)}, {'D', (3, 2)}, {'F', (3, 2)}, {'G', (3, 2)},
        {'H', (3, 2)}, {'J', (3, 2)}, {'K', (3, 2)}, {'L', (3, 2)}, {'M', (3, 2)},
        {'N', (3, 2)}, {'P', (3, 2)}, {'R', (3, 2)}, {'S', (3, 2)}, {'T', (3, 2)},
        {'V', (3, 2)}, {'W', (3, 2)}, {'Y', (3, 2)}
    };

    public static int CalculateAttackPower(string weapon)
    {
        if (string.IsNullOrEmpty(weapon))
            return 0;

        int totalAttack = 0;
        foreach (char c in weapon.ToUpper())
        {
            if (letterStats.TryGetValue(c, out var stats))
            {
                totalAttack += stats.attack;
            }
        }
        return totalAttack;
    }

    public static int CalculateDefensePower(string shield)
    {
        if (string.IsNullOrEmpty(shield))
            return 0;

        int totalDefense = 0;
        foreach (char c in shield.ToUpper())
        {
            if (letterStats.TryGetValue(c, out var stats))
            {
                totalDefense += stats.defense;
            }
        }
        return totalDefense;
    }

    public static (int attack, int defense) GetLetterStats(char letter)
    {
        char upperLetter = char.ToUpper(letter);
        if (letterStats.TryGetValue(upperLetter, out var stats))
        {
            return stats;
        }
        return (0, 0);
    }
}
