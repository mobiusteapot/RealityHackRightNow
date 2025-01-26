using UnityEngine;
using UnityEngine.UI;

public class RandomTopicGenerator : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private InputField inputField; // Reference to the InputField
    [SerializeField] private Button generateButton; // Reference to the Button

    [Header("Word List")]
    public string[] words = new string[]
    {
        "Backlog",
        "Sprint",
        "Deliverables",
        "Retrospective",
        "Kanban",
        "Velocity",
        "Stakeholders",
        "Dependencies",
        "Roadmap",
        "Standup",
        "MVP",
        "Iteration",
        "Burndown",
        "UserStories",
        "TechDebt",
        "Release",
        "Prioritization",
        "Epics",
        "Workflow",
        "BlockingIssues"
    };


    private void Start()
    {
        // Ensure the button has a listener assigned
        if (generateButton != null)
        {
            generateButton.onClick.AddListener(GenerateRandomWord);
        }
    }

    private void GenerateRandomWord()
    {
        if (inputField == null)
        {
            Debug.LogError("InputField reference is missing.");
            return;
        }

        // Pick a random word from the array
        string randomWord = words[Random.Range(0, words.Length)];

        // Assign the random word to the InputField's text
        inputField.text = randomWord;
    }
}
