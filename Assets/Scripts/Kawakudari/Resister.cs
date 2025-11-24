using UnityEngine;

public class Resister : MonoBehaviour
{
    void Awake()
    {
        GetIt.Instance.Register<IGameData, KawaGameData>(new KawaGameData());
    }
}
