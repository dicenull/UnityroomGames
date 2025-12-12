using R3;
using Unity.VisualScripting;
using UnityEngine;

public class InvEnemyController : MonoBehaviour
{
    float timer = 0f;
    float coolDown = .3f;


    void Start()
    {
        var gameData = GetIt.Instance.Get<InvGameData>();
        gameData.OnHit.ThrottleFirst(System.TimeSpan.FromSeconds(coolDown)).Subscribe(nextPhase =>
        {
            gameData.MovePhase.Value = nextPhase;
            // 一段下に移動
            transform.position += new Vector3(0, -.3f, 0);
            timer = 0f;
        }).AddTo(this);
    }

    // 最初は、右に移動
    // 右端まできたら折り返して左に移動
    // 左端まで来たら、折り返して右に移動
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= coolDown)
        {
            var gameData = GetIt.Instance.Get<InvGameData>();
            var moveAmount = gameData.MoveAmount;
            transform.position += moveAmount;
            timer = 0f;
        }
    }
}
