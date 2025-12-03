using UnityEngine;

public class ChargeGameDataRegister : MonoBehaviour
{
    void Awake()
    {
        GetIt.Instance.Register<IGameData, ChargeGameData>(new ChargeGameData());
    }

}
