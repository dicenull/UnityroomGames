using UnityEngine;

public class ChargeGameDataRegister : MonoBehaviour
{

    void Start()
    {
        GetIt.Instance.Register<IGameData, ChargeGameData>(new ChargeGameData());
    }

}
