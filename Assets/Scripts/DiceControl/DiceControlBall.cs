using UnityEngine;

public class DiceControlBall : MonoBehaviour
{
    float lifeTime = 20f;
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // だんだん小さく。lifeTimeで消える
        transform.localScale *= 1f - Time.deltaTime / lifeTime;
    }
}
