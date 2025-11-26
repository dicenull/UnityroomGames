using UnityEngine;

public class DiceControlRegister : MonoBehaviour
{
    void Awake()
    {
        GetIt.Instance.Register<IGameData, DiceControlGameData>(new DiceControlGameData());
    }
}
