using UnityEngine;

public class InvCheckGameOver : MonoBehaviour
{
    void Update()
    {
        int count = 0;
        foreach (var child in transform)
        {
            count++;
        }

        if (count == 0)
        {
            Debug.Log("GameOver");
            var gameData = GetIt.Instance.Get<InvGameData>();
            gameData.GameOver();
        }
    }

}
