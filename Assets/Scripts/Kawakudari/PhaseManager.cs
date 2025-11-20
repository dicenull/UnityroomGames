using UnityEngine;
using UnityEngine.InputSystem;

public class PhaseManager : MonoBehaviour
{

    [SerializeField] private GameObject startPhaseObject;

    void Start()
    {
        startPhaseObject.SetActive(true);
    }

    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard.enterKey.wasPressedThisFrame)
        {
            var gameData = GetIt.Instance.Get<KawaGameData>();
            gameData.GameStart();
            startPhaseObject.SetActive(false);
        }
    }
}
