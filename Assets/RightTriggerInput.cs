using System;
using System.Text;
using UnityEngine;
using Meta.Voice.Samples.Dictation;
using Meta.WitAi.Dictation;
using TMPro;
using Utilities.Extensions;

public class RightTriggerInput : MonoBehaviour
{
    public DictationActivation _dictationActivation;
    [SerializeField] private DictationService _dictationService;
    [SerializeField] private TextMeshProUGUI _input;
    [SerializeField] private String _msg;
    [SerializeField] private BranchAi _branchAi;

    void Start()
    {
        _dictationActivation = FindObjectOfType<DictationActivation>();
        _dictationService = FindObjectOfType<DictationService>();
        _input = FindObjectOfType<MultiRequestTranscription>().GetComponent<TextMeshProUGUI>();
        _branchAi = FindObjectOfType<BranchAi>();
        if (_input != null)
        {
            Debug.Log("inputField found");
        }
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
                if (!_dictationService.MicActive)
                {
                    _msg = ParseTranscript(_input.text);
                    //TODO: process text
                    Debug.Log(_msg);
                    _branchAi.Summarize(_msg);
                }
            }
        }

    string ParseTranscript(string rawTranscript)
    {
        if (string.IsNullOrEmpty(rawTranscript))
        {
            return string.Empty;
        }

        // Split the transcript into lines
        string[] lines = rawTranscript.Split(new[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

        // Combine lines while ignoring the delimiter (;;)
        StringBuilder cleanedMessage = new StringBuilder();
        foreach (string line in lines)
        {
            string trimmedLine = line.Trim(); // Remove leading and trailing spaces

            // Skip the delimiter (;;)
            if (trimmedLine == ";;")
            {
                continue;
            }

            // Add the line to the final message
            if (cleanedMessage.Length > 0)
            {
                cleanedMessage.Append(" "); // Add a space between sentences
            }

            cleanedMessage.Append(trimmedLine);
        }

        return cleanedMessage.ToString();
    }
}