using UnityEngine;

public class InvDataRegister : MonoBehaviour
{
    void Awake()
    {
        GetIt.Instance.Register<IGameData, InvGameData>(new InvGameData());
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init()
    {
        PlayerPrefs.DeleteKey("CoolDown");
    }
}
