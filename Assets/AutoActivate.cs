using Meta.Voice.Samples.Dictation;
using UnityEngine;

public class AutoActivate : MonoBehaviour
{
    [SerializeField]private DictationActivation _dictationActivation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _dictationActivation = GetComponent<DictationActivation>();
        if (_dictationActivation != null)
        {
            _dictationActivation.ToggleActivation(); 
            Debug.Log("Dictation Activated...");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
