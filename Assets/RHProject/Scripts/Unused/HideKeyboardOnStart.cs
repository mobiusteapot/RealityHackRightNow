using UnityEngine;

[RequireComponent(typeof(OVRVirtualKeyboard))]
public class HideKeyboardOnStart : MonoBehaviour
{
    [SerializeField]
    private OVRVirtualKeyboard keyboard;

    private void Reset()
    {
        TryGetComponent(out keyboard);
    }
    private void Start()
    {
        keyboard.enabled = false;
    }
}
