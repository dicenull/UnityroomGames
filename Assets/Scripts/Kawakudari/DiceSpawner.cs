using UnityEngine;

public class DiceSpawner : MonoBehaviour
{
    float coolDown = 0.3f;
    float timer = 0f;


    void Update()
    {
        var gameData = GetIt.Instance.Get<KawaGameData>();
        if (gameData.IsGameStart.Value)
        {
            Spawn();
        }
    }

    void Spawn()
    {
        timer += Time.deltaTime;
        if (timer >= coolDown)
        {
            var randPos = new Vector3(Random.Range(-8f, 8f), transform.position.y, 0f);
            var randRotate = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
            var dice = Instantiate(Resources.Load<GameObject>("dice"), randPos, randRotate);
            dice.transform.localScale = Vector3.one * 0.7f;

            Destroy(dice, 10f);

            timer = 0f;
        }
    }
}
