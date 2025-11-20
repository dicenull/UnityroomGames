
using UnityEngine;

public class DiceController : MonoBehaviour
{
    float speed = 10f;
    void Update()
    {
        transform.position += new Vector3(0f, -speed * Time.deltaTime, 0f);
    }
}