using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float Speed = 10f;

    void Update()
    {
        var key = Keyboard.current;
        if (key[Key.LeftArrow].isPressed)
        {
            Move(Vector3.left);
        }

        if (key[Key.RightArrow].isPressed)
        {
            Move(Vector3.right);
        }

        CheckOverflow();
    }

    void CheckOverflow()
    {
        var pos = transform.position;
        var threshold = 10f;
        if (Math.Abs(pos.x) > threshold)
        {
            pos.x *= -1;
        }
        transform.position = pos;
    }

    void Move(Vector3 direction)
    {
        transform.position += direction * Speed * Time.deltaTime;
    }
}
