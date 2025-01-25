using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CanvasButtonSimulator : MonoBehaviour
{
    // Reference to the Unity Canvas UI Button you want to simulate
    public Button targetButton;

    private void Reset()
    {
        TryGetComponent(out targetButton);
    }
}
