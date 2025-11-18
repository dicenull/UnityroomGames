using System;
using Microsoft.Extensions.Primitives;
using UnityEngine;
using UnityEngine.InputSystem;

public class BattleInput : MonoBehaviour
{
    private BattleManager battleManager;
    private Key weaponKey;
    private Key shieldKey;
    private Key potionKey;
    void Awake()
    {
        battleManager = FindObjectOfType<BattleManager>();
        weaponKey = StringToKey.FromString(GameData.Instance.Weapon.Value);
        shieldKey = StringToKey.FromString(GameData.Instance.Shield.Value);

        Debug.Log($"weapon: {weaponKey}, shield: {shieldKey}");
    }

    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard[weaponKey].wasPressedThisFrame)
        {
            battleManager.PlayerAttack();
        }
        if (keyboard[shieldKey].wasPressedThisFrame)
        {
            battleManager.PlayerDefend();
        }
    }
}
