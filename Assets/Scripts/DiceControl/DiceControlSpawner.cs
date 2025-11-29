using UnityEngine;

public class DiceControlSpawner : MonoBehaviour
{
    float timer = 0f;
    float coolDown = .1f;

    void Update()
    {
        var gameData = GetIt.Instance.Get<DiceControlGameData>();
        if (!gameData.IsGameStart.Value || gameData.IsGameOver.Value) return;

        timer += Time.deltaTime;
        if (timer >= coolDown)
        {
            var randPos = new Vector3(Random.Range(-20f, 20f), Random.Range(-20f, 20f), Random.Range(-20f, 20f));
            Instantiate(Resources.Load<GameObject>("Ball"), randPos, Quaternion.identity);

            timer = 0f;
        }
    }
}
