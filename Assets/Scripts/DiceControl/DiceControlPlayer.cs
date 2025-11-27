
using System.Collections.Generic;
using R3;
using UnityEngine;
using UnityEngine.InputSystem;

public class DiceControlPlayer : MonoBehaviour
{
    [SerializeField] private float Speed = 30f;
    new Rigidbody rigidbody;

    void OnCollisionEnter(Collision collision)
    {
        var other = collision.gameObject;

        if (other.transform.parent.name == "Wall")
        {
            GetIt.Instance.Get<IGameData>().GameOver();
        }
    }

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // wasd 入力方向に移動
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        // サイコロの面に対応するキー入力で移動
        // 1の面が前
        var key2Angle = new Dictionary<Key, Quaternion> { { Key.Digit1, Quaternion.identity },
                                                                 { Key.Digit2, Quaternion.Euler(90, 0, 0) },
                                                                 { Key.Digit3, Quaternion.Euler(0, -90, 0) },
                                                                 { Key.Digit4, Quaternion.Euler(0, 90,0) },
                                                                 { Key.Digit5, Quaternion.Euler(-90, 0, 0) },
                                                                 { Key.Digit6, Quaternion.Euler(0,180, 0) } };
        var gameData = GetIt.Instance.Get<DiceControlGameData>();
        var diceCount = new Dictionary<Key, ReactiveProperty<int>>
        {
            { Key.Digit1, gameData.DiceCountOne },
            { Key.Digit2, gameData.DiceCountTwo },
            { Key.Digit3, gameData.DiceCountThree },
            { Key.Digit4, gameData.DiceCountFour },
            { Key.Digit5, gameData.DiceCountFive },
            { Key.Digit6, gameData.DiceCountSix },
        };

        foreach (var kvp in key2Angle)
        {
            if (keyboard[kvp.Key].wasPressedThisFrame)
            {
                if (diceCount[kvp.Key].Value <= 0)
                {
                    // カウントが0以下なら移動しない
                    break;
                }
                diceCount[kvp.Key].Value--;

                var direction = kvp.Value * transform.forward;
                Move(direction);
                break;
            }
        }
    }

    void Move(Vector3 direction)
    {
        rigidbody.linearVelocity = Vector3.zero;
        rigidbody.AddForce(direction * Speed, ForceMode.VelocityChange);
    }
}