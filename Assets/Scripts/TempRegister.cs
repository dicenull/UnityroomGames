using R3;
using UnityEngine;

public class MyGameData : IGameData
{
    public ReactiveProperty<bool> IsGameOver => new(false);

    Observable<bool> IGameData.IsGameOver => IsGameOver;

    public void GameOver()
    {
        IsGameOver.Value = true;
    }

    public void Reset()
    {
        IsGameOver.Value = false;
    }
}

public class TempRegister : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetIt.Instance.Register<IGameData, MyGameData>(new MyGameData());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
