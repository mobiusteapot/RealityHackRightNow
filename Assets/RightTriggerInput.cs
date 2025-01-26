using UnityEngine;
using Meta.Voice.Samples.Dictation;
public class RightTriggerInput : MonoBehaviour
{
    public DictationActivation _dictationActivation;

    void Start()
    {
        _dictationActivation = FindObjectOfType<DictationActivation>();
    }
    void Update()
    {
        // Get the analog value of the right controller's trigger
        float triggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch);

        // Check if the trigger is pressed beyond a threshold
        if (triggerValue > 0.1f) // Adjust the threshold if needed
        {
            Debug.Log("Right Trigger is being pressed!");
        }

        // Check if the trigger is fully pressed (button press)
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            _dictationActivation.ToggleActivation();
        }
    }
}