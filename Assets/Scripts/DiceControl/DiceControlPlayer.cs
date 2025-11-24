using UnityEngine;

public class DiceControlPlayer : MonoBehaviour
{
    [SerializeField] private float Speed = 20f;

    void Update()
    {
        Move(transform.forward);
    }

    void Move(Vector3 direction)
    {
        var rigidbody = GetComponent<Rigidbody>();
        rigidbody.AddForce(direction * Speed * Time.deltaTime);
    }
}