using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(MeshRenderer))]
public class Player : MonoBehaviour
{
    [SerializeField] private Material FreezeMaterial;
    [SerializeField] private Material NormalMaterial;
    [SerializeField] private float Speed = 10f;
    private float freezeTime = .5f;
    private float freezeTimer = 0f;
    private bool isFreeze = false;

    void OnCollisionEnter(Collision collision)
    {
        GetIt.Instance.Get<IGameData>().GameOver();
    }

    void Update()
    {
        var gameData = GetIt.Instance.Get<KawaGameData>();

        CheckOverflow();
        if (Time.time - freezeTimer > freezeTime)
        {
            UnFreeze();
        }

        var key = Keyboard.current;
        if (!isFreeze)
        {
            if (key[Key.LeftArrow].isPressed)
            {
                Move(Vector3.left);
            }

            if (key[Key.RightArrow].isPressed)
            {
                Move(Vector3.right);
            }
        }

        if (gameData.IsGameStart.Value &&
            key[Key.Space].wasPressedThisFrame)
        {
            Freeze();
        }
    }

    void UnFreeze()
    {
        isFreeze = false;
        GetComponent<MeshRenderer>().material = NormalMaterial;
    }

    void Freeze()
    {
        var gameData = GetIt.Instance.Get<KawaGameData>();
        gameData.AddFreeze();

        freezeTimer = Time.time;
        isFreeze = true;
        GetComponent<MeshRenderer>().material = FreezeMaterial;
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
