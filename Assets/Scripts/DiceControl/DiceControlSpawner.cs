using UnityEngine;

public class DiceControlSpawner : MonoBehaviour
{
    float timer = 0f;
    float coolDown = .1f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= coolDown)
        {
            var randPos = new Vector3(Random.Range(-20f, 20f), Random.Range(-20f, 20f), Random.Range(-20f, 20f));
            var dice = Instantiate(Resources.Load<GameObject>("Ball"), randPos, Quaternion.identity);
            Destroy(dice, 20f);

            timer = 0f;
        }
    }
}
