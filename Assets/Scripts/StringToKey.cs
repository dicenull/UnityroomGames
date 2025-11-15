
using UnityEngine.InputSystem;

public static class StringToKey
{
    public static Key FromString(string name)
    {
        return Key.A + (name.ToUpper()[0] - 'A');
    }
}