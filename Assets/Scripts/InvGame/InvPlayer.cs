using R3;
using R3.Triggers;
using UnityEngine;
using UnityEngine.InputSystem;

public class InvPlayer : MonoBehaviour
{
    [SerializeField] GameObject bullet;

    bool canSingleShot = true;

    void Update()
    {
        var gameData = GetIt.Instance.Get<InvGameData>();
        var keyboard = Keyboard.current;

        var amount = new Vector3(.1f, 0, 0);
        if (keyboard[Key.A].isPressed)
        {
            transform.position -= amount;
        }
        if (keyboard[Key.D].isPressed)
        {
            transform.position += amount;
        }

        if (canSingleShot && keyboard[Key.Space].isPressed)
        {
            canSingleShot = false;

            var shot = Instantiate(bullet, transform.position, Quaternion.identity);
            Destroy(shot, 2f);

            var rigidbody = shot.AddComponent<Rigidbody>();
            rigidbody.useGravity = false;
            rigidbody.AddForce(new Vector3(0, 10, 0), ForceMode.Impulse);

            Invoke("ResetCoolDown", gameData.ShotCoolDown.Value);
        }
    }

    public void ResetCoolDown()
    {
        canSingleShot = true;
    }
}
