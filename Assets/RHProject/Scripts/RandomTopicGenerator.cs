using UnityEngine;
using UnityEngine.UI;

public class RandomTopicGenerator : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private InputField inputField; // Reference to the InputField
    [SerializeField] private Button generateButton; // Reference to the Button

    [Header("Word List")]
    [SerializeField] private string[] words = new string[]
    {
        "Apple", "Banana", "Cherry", "Dragonfruit", "Elderberry",
        "Fig", "Grape", "Honeydew", "Kiwi", "Lemon", "Mango", "Nectarine",
        "Orange", "Papaya", "Quince", "Raspberry", "Strawberry", "Tomato",
        "Ugli", "Vanilla", "Watermelon", "Xigua", "Yam", "Zucchini"
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
