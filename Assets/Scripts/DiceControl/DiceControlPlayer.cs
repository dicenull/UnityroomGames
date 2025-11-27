
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

        var keys = new Key[] { Key.A, Key.D, Key.W, Key.S };
        var angles = new float[] { -90, 90, 0, 180 };
        for (int i = 0; i < keys.Length; i++)
        {
            if (keyboard[keys[i]].wasPressedThisFrame)
            {
                var angle = angles[i];
                var direction = Quaternion.Euler(0, angle, 0) * transform.forward;
                Move(direction);
            }
        }


    }

    void Move(Vector3 direction)
    {
        rigidbody.linearVelocity = Vector3.zero;
        rigidbody.AddForce(direction * Speed, ForceMode.VelocityChange);
    }
}