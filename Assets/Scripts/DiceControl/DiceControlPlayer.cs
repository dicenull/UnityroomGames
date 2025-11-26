using UnityEngine;
using UnityEngine.InputSystem;

public class DiceControlPlayer : MonoBehaviour
{
    [SerializeField] private float Speed = 30f;

    void Update()
    {
        // wasd 入力方向に移動
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        Vector3 vec = Vector3.zero;
        var keys = new Key[] { Key.A, Key.D, Key.W, Key.S };
        var dir = new Vector3[] { Vector3.left, Vector3.right, Vector3.forward, Vector3.back };
        for (int i = 0; i < keys.Length; i++)
        {
            if (keyboard[keys[i]].isPressed)
            {
                vec = dir[i];
            }
        }
        Debug.Log($"Input: {vec}");

        Move(vec);
    }

    void Move(Vector3 direction)
    {
        var rigidbody = GetComponent<Rigidbody>();
        rigidbody.AddForce(direction * Speed * Time.deltaTime);
    }
}