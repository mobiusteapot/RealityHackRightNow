using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Button))]
public class CanvasButtonSimulator : MonoBehaviour
{
    public enum ControllerInputSelection{
        QuestLeftTop,
        QuestLeftBottom,
        QuestRightTop,
    }
    public ControllerInputSelection controllerInputSelection;

    public InputActionReference buttonInput;
    // Reference to the Unity Canvas UI Button you want to simulate
    public Button targetButton;

    private void Reset()
    {
        TryGetComponent(out targetButton);
    }
    private void Update(){
        // Listen for OVRInput button presses
        if(controllerInputSelection == ControllerInputSelection.QuestLeftBottom){
            if(OVRInput.GetDown(OVRInput.Button.Three)){
                targetButton.onClick.Invoke();
            }
        }
        else if(controllerInputSelection == ControllerInputSelection.QuestLeftTop){
            if(OVRInput.GetDown(OVRInput.Button.Four)){
                targetButton.onClick.Invoke();
            }
        }
        else if(controllerInputSelection == ControllerInputSelection.QuestRightTop){
            if(OVRInput.GetDown(OVRInput.Button.One)){
                targetButton.onClick.Invoke();
            }
        }
    }
    private void OnEnable()
    {
        if(buttonInput == null)
        {
            return;
        }
        buttonInput.action.Enable();
        buttonInput.action.performed += OnButtonPressed;
    }

    private void OnDisable()
    {
        if(buttonInput == null)
        {
            return;
        }
        buttonInput.action.performed -= OnButtonPressed;
    }

    private void OnButtonPressed(InputAction.CallbackContext context)
    {
        Debug.Log("Button pressed!: " + buttonInput.action.name);
        targetButton.onClick.Invoke();
    }
}
