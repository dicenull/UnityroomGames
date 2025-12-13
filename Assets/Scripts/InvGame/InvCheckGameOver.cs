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
            var gameData = GetIt.Instance.Get<InvGameData>();
            if (!gameData.IsGameOver.Value)
            {
                gameData.GameOver();
            }
        }
    }

}
